using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
  {
    public void Configure(EntityTypeBuilder<UserConnection> builder)
    {
      builder.HasKey(x => new {x.Email});
      builder.ToTable("UserConnection");
      builder.Property(x => x.RefreshTokenExpiredDate).HasDefaultValue(DateTime.Now.AddDays(7));
      builder.Property(x => x.AccessTokenExpiredDate).HasDefaultValue(DateTime.Now.AddMinutes(15));
      builder.Property(x => x.AccessToken).HasMaxLength(2048);
    }
  }
}