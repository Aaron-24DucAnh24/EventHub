using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class CategoryConfiguration: IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      builder.HasKey(x=>x.Id);
      builder.Property(x=>x.Id).HasDefaultValue(Guid.NewGuid().ToString());
      builder.ToTable("Category");
			builder.Property(x=>x.Name).IsRequired();
      builder.Property(x=>x.UpdatedAt).HasDefaultValue(null);
      builder.Property(x=>x.DeletedAt).HasDefaultValue(null);
      builder.Property(x=>x.CreatedAt).HasDefaultValue(DateTime.Now);
      builder.Property(x=>x.IsDeleted).HasDefaultValue(false);
    }
  }
}