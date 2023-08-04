using Microsoft.EntityFrameworkCore;
using TicketBooking.API.DBContext.Configuration;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext
{
  public class ApplicationDbContext: DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new EventConfiguration());
      modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
      modelBuilder.ApplyConfiguration(new SeatConfiguration());
      modelBuilder.ApplyConfiguration(new CategoryConfiguration());
      modelBuilder.ApplyConfiguration(new EventCategoryConfiguration());
      modelBuilder.ApplyConfiguration(new SeatEventConfiguration());
      modelBuilder.ApplyConfiguration(new SeatInvoiceConfiguration());
      
      DbContextSeeding.Run(ref modelBuilder);
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<SeatEvent> SeatEvents { get; set; }
    public DbSet<SeatInvoice> SeatInvoices { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
  }
}