using FluentValidation;
using Microsoft.OpenApi.Models;
using TicketBooking.API.Dtos;
using TicketBooking.API.Dtos.Validators;
using TicketBooking.API.Repository;
using TicketBooking.API.Services;

namespace TicketBooking.API
{
  public static class ServicesExtension
  {
    public static void AddBusinessServices(this IServiceCollection services)
    {
      services.AddScoped<IEventService, EventService>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<IInvoiceService, InvoiceService>();
      services.AddSingleton<IEmailValidationService, EmailValidationService>();
      services.AddScoped<ICacheService, CacheService>();
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
}