using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Enum;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class SeatInvoiceConfiguration: IEntityTypeConfiguration<SeatInvoice>
  {
    public void Configure(EntityTypeBuilder<SeatInvoice> builder)
    {
      builder.HasKey(x=>new{x.InvoiceId, x.SeatId});
      builder.ToTable("SeatInvoice");
    }
  }
}