using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TicketBooking.API.Contexts;
using TicketBooking.API.Dtos;
using TicketBooking.API.Dtos.Validators;
using TicketBooking.API.Enums;
using TicketBooking.API.Helper;
using TicketBooking.API.Repository;
using TicketBooking.API.Services;

namespace TicketBooking.API.Extensions
{
  public static class ServicesExtension
  {
    public static void AddBusinessServices(this IServiceCollection services)
    {
      services.AddScoped<IEventService, EventService>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<IInvoiceService, InvoiceService>();
      services.AddSingleton<IEmailValidationService, EmailValidationService>();
      services.AddSingleton<ICacheService, CacheService>();
      services.AddScoped<IBlobService, BlobService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<ICookieService, CookieService>();
      services.AddScoped<IValidator<EventRequest>, EventRequestValidator>();
      services.AddScoped<IValidator<InvoiceRequest>, InvoiceRequestValidator>();
      services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
      services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public static void AddRepositoryServices(this IServiceCollection services)
    {
      services.AddScoped<IEventRepository, EventRepository>();
      services.AddScoped<ICategoryRepository, CategoryRepository>();
      services.AddScoped<ISeatRepository, SeatRepository>();
      services.AddScoped<IInvoiceRepository, InvoiceRepository>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IUserConnectionRepository, UserConnectionRepository>();
    }

    public static void AddCoreServices(this IServiceCollection services)
    {
      services.AddSwaggerService();
      services.AddCorsPolicies();
      services.AddJwtAuthentication();
      services.AddHttpContextAccessor();
    }

    public static void AddBusinessContexts(this IServiceCollection services)
    {
      services.AddScoped<IUserContext, UserContext>();
    }

    public static void AddSwaggerService(this IServiceCollection services)
    {
      services.AddSwaggerGen(options =>
      {
        options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        });

        options.OperationFilter<SecureEndpointAuthRequirementFilter>();
      });
    }

    public static void AddCorsPolicies(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("public", policy =>
        {
          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
      });
    }

    public static void AddJwtAuthentication(this IServiceCollection services)
    {
      services
        .AddAuthentication(opt =>
        {
          opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.SaveToken = true;
          options.TokenValidationParameters = TokenHelper.CreateTokenValidationParameters();
          options.Events = new JwtBearerEvents()
          {
            OnMessageReceived = context =>
            {
              Endpoint? endpoint = context.HttpContext.GetEndpoint();
              bool authorized = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null;

              if (authorized)
              {
                string? accessToken = TokenHelper.GetAccessTokenFromRequest(context.Request) ?? throw new SecurityTokenValidationException();
                IAuthService authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                bool isValid = authService.ValidateAccessToken(accessToken);

                if (isValid)
                {
                  context.Token = accessToken;
                }
                else
                {
                  throw new SecurityTokenValidationException();
                }
              }

              return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
              ClaimsPrincipal? claimsPrincipal = context.Principal ?? throw new SecurityTokenValidationException();
              UserContextInfo userContextInfo = TokenHelper.GetEmail(claimsPrincipal) ?? throw new SecurityTokenValidationException();
              
              IUserContext userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();
              userContext.Email = userContextInfo.Email; 
              userContext.Name = userContextInfo.Name;
              userContext.UserRole = userContextInfo.Role;

              return Task.CompletedTask;
            }
          };
        });
    }
  }

  internal class SecureEndpointAuthRequirementFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      if (!context.ApiDescription
        .ActionDescriptor
        .EndpointMetadata
        .OfType<AuthorizeAttribute>()
        .Any())
      {
        return;
      }

      operation.Security = new List<OpenApiSecurityRequirement>
      {
        new OpenApiSecurityRequirement
        {
          [new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            Name = "Bearer",
            In = ParameterLocation.Header,
          }] = new List<string>()
        }
      };
    }
  }
}