using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketBooking.API.Enum;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext.Configuration
{
  public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
  {
    public void Configure(EntityTypeBuilder<UserConnection> builder)
    {
      builder.HasKey(x => new {x.RefreshToken, x.AccessToken});
      builder.ToTable("UserConnection");
      builder.Property(x => x.IsDeleted).HasDefaultValue(false);
    }
  }
}