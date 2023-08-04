using Microsoft.EntityFrameworkCore;
using TicketBooking.API.DBContext;
using TicketBooking.API.Services;
using TicketBooking.API.Repository;
using FluentValidation;
using TicketBooking.API.Dtos.Validators;
using TicketBooking.API.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling
        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IEmailValidationService, EmailValidationService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

builder.Services.AddScoped<IValidator<EventRequest>, EventRequestValidator>();
builder.Services.AddScoped<IValidator<InvoiceRequest>, InvoiceRequestValidator>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("public", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("TicketBookingDatabase"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("public");

app.UseAuthorization();

app.MapControllers();

app.Run();
