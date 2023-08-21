using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Constants;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
  {
    public void Configure(EntityTypeBuilder<UserConnection> builder)
    {
      builder.HasKey(x => new { x.Email });
      builder.ToTable("UserConnection");
      builder.Property(x => x.RefreshTokenExpiredDate)
        .HasDefaultValue(DateTimeOffset.Now.AddMinutes(TokenSettings.REFRESH_TOKEN_EXPIRATIONS_MINUTES));
      builder.Property(x => x.RefreshToken).HasDefaultValue(string.Empty);
      builder.Property(x => x.AccessTokenExpiredDate)
        .HasDefaultValue(DateTimeOffset.Now.AddMinutes(TokenSettings.ACCESS_TOKEN_EXPIRATION_MINUTES));
      builder.Property(x => x.AccessToken).HasDefaultValue(string.Empty);
      builder.Property(x => x.AccessToken).HasMaxLength(2048);
      builder.Property(x => x.IsDeleted).HasDefaultValue(false);
    }
  }
}