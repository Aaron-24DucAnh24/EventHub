using Microsoft.EntityFrameworkCore;
using TicketBooking.API.Extensions;
using TicketBooking.API.DBContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllersWithViews()
	.AddNewtonsoftJson(options =>
		options.SerializerSettings.ReferenceLoopHandling
		= Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddBusinessServices();

builder.Services.AddRepositoryServices();

builder.Services.AddCoreService();

builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();

app.UseCors("public");

app.UseAuthorization();

app.MapControllers();

app.Run();
