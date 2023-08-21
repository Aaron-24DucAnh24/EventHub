using TicketBooking.API.Enums;

namespace TicketBooking.API.Dtos
{
  public class UserContextInfo
  {
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
  }
}