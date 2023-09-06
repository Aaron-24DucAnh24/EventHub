using TicketBooking.API.Enums;

namespace TicketBooking.API.Contexts
{
  public interface IUserContext
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole UserRole { get; set; }
  }
}