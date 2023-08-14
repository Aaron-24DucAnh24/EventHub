using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Enum;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("User");
      builder.Property(x => x.Role).HasDefaultValue(UserRole.AppUser);
      builder.Property(x => x.UpdatedAt).HasDefaultValue(null);
      builder.Property(x => x.DeletedAt).HasDefaultValue(null);
      builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.Now);
      builder.Property(x => x.IsDeleted).HasDefaultValue(false);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Email).IsRequired();
      builder.Property(x => x.Password).IsRequired();
    }
  }
}