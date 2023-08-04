using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
	class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
	{
		public void Configure(EntityTypeBuilder<Invoice> builder)
		{
			builder.HasKey(x=>x.Id);
			builder.ToTable("Invoice");
      builder.Property(x=>x.Id).HasDefaultValue(Guid.NewGuid().ToString());
			builder.Property(x=>x.Name).IsRequired();
			builder.Property(x=>x.Mail).IsRequired();
			builder.Property(x=>x.Phone).IsRequired();
			builder.Property(x=>x.UpdatedAt).HasDefaultValue(null);
      builder.Property(x=>x.DeletedAt).HasDefaultValue(null);
      builder.Property(x=>x.CreatedAt).HasDefaultValue(DateTime.Now);
      builder.Property(x=>x.IsDeleted).HasDefaultValue(false);
			builder.Property(x=>x.Code).HasDefaultValue("");
			builder.Property(x=>x.IsValidated).HasDefaultValue(false);
      builder.Property(x=>x.EventId).IsRequired();

			builder.HasMany(x=>x.Seats).WithMany(e=>e.Invoices).UsingEntity<SeatInvoice>(
        x=>x.HasOne<Seat>().WithMany().HasForeignKey(x=>x.SeatId),
        x=>x.HasOne<Invoice>().WithMany().HasForeignKey(x=>x.InvoiceId)
      );
		}
	}
}