using TicketBooking.API.Enums;

namespace TicketBooking.API.Contexts
{
  public class UserContext : IUserContext
  {
    private string _name = "";
    private string _email = "";
    private UserRole _role;

    public string Name { get { return _name; } set { _name = value; } }
    public string Email { get { return _email; } set { _email = value; } }
    public UserRole UserRole { get { return _role; } set { _role = value; } }
  }
} 