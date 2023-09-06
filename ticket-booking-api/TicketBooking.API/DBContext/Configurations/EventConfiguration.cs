using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Models;
using TicketBooking.API.Enums;

namespace TicketBooking.API.DBContext.Configuration
{
  public class EventConfiguration: IEntityTypeConfiguration<Event>
  {
    public void Configure(EntityTypeBuilder<Event> builder)
    {
      builder.HasKey(x=>x.Id);
      builder.Property(x=>x.Id).HasDefaultValue(Guid.NewGuid().ToString());
      builder.ToTable("Event");
      builder.Property(x=>x.Status).HasDefaultValue(EventStatus.Available);
      builder.Property(x=>x.UpdatedAt).HasDefaultValue(null);
      builder.Property(x=>x.DeletedAt).HasDefaultValue(null);
      builder.Property(x=>x.CreatedAt).HasDefaultValue(DateTime.Now);
      builder.Property(x=>x.IsDeleted).HasDefaultValue(false);
      builder.Property(x=>x.IsPublished).HasDefaultValue(false);
      builder.Property(x=>x.StageName).IsRequired();
      builder.Property(x=>x.Image).HasDefaultValue("");
      builder.Property(x=>x.MinPrice).IsRequired();
      builder.Property(x=>x.Title).IsRequired().IsUnicode();
      builder.Property(x=>x.Date).HasDefaultValue(DateTime.Parse("8/11/2023 9:15:00 PM"));
      builder.Property(x=>x.Location).IsRequired();
      builder.Property(x=>x.City).HasDefaultValue(null).IsUnicode();

      builder.HasMany(x=>x.Categories).WithMany(e=>e.Events).UsingEntity<EventCategory>(
        ec=>ec.HasOne<Category>().WithMany().HasForeignKey(ec=>ec.CategoryId),
        ec=>ec.HasOne<Event>().WithMany().HasForeignKey(ec=>ec.EventId)
      );

      builder.HasMany(x=>x.Invoices).WithOne(x=>x.Event);
    }
  }
}