using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TicketBooking.API.Dtos;
using TicketBooking.API.Dtos.Validators;
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

    public static void AddCoreService(this IServiceCollection services)
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

      string? issuer = ConfigurationHelper.configuration.GetValue<string>("Token:Issuer");
      string? signingKey = ConfigurationHelper.configuration.GetValue<string>("Token:Key");
      if (signingKey == null)
      {
        throw new ArgumentException("No key configuration found");
      }
      byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidIssuer = issuer,
          ValidateAudience = true,
          ValidAudience = issuer,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ClockSkew = TimeSpan.Zero,
          IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
      });

      services.AddCors(options =>
      {
        options.AddPolicy("public", policy =>
        {
          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
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