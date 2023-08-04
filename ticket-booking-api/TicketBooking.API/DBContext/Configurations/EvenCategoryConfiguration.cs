using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class EventCategoryConfiguration: IEntityTypeConfiguration<EventCategory>
  {
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
      builder.HasKey(x=>new {x.EventId, x.CategoryId});
      builder.ToTable("EventCategory");
    }
  }
}