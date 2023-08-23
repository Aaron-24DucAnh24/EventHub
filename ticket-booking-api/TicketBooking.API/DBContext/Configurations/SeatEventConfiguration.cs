using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Enums;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class SeatEventConfiguration: IEntityTypeConfiguration<SeatEvent>
  {
    public void Configure(EntityTypeBuilder<SeatEvent> builder)
    {
      builder.HasKey(x=>new{x.EventId, x.SeatId});
      builder.ToTable("SeatEvent");
			builder.Property(x=>x.SeatStatus).HasDefaultValue(SeatStatus.Free);
			builder.Property(x=>x.Price).IsRequired();
      builder.HasOne(x => x.Event).WithMany(x=>x.SeatEvents).HasForeignKey(x=>x.EventId);
      builder.HasOne(x => x.Seat).WithMany(x=>x.SeatEvents).HasForeignKey(x=>x.SeatId);
    }
  }
}