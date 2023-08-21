using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Enums;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
	class SeatConfiguration : IEntityTypeConfiguration<Seat>
	{
		public void Configure(EntityTypeBuilder<Seat> builder)
		{
			builder.HasKey(x=>x.Id);
      builder.Property(x=>x.Id).HasDefaultValue(Guid.NewGuid().ToString());
			builder.ToTable("Seat");
			builder.Property(x=>x.Name).IsRequired();
			builder.Property(x=>x.Type).HasDefaultValue(SeatType.Standard);
			builder.Property(x=>x.UpdatedAt).HasDefaultValue(null);
      builder.Property(x=>x.DeletedAt).HasDefaultValue(null);
      builder.Property(x=>x.CreatedAt).HasDefaultValue(DateTime.Now);
      builder.Property(x=>x.IsDeleted).HasDefaultValue(false);

			builder.HasMany(x=>x.Invoices).WithMany(e=>e.Seats).UsingEntity<SeatInvoice>(
        x=>x.HasOne<Invoice>().WithMany().HasForeignKey(x=>x.InvoiceId),
        x=>x.HasOne<Seat>().WithMany().HasForeignKey(x=>x.SeatId)
      );
		}
	}
}