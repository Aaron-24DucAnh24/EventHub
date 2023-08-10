using System.Text;

namespace TicketBooking.API.Helper
{
  public class HashHelper
  {
    public static string GetHash(string password)
    {
      return password;
    }

    public static bool CompareHash(string attemptedPassword, string hashedPassword)
    {
      return true;
    }
  }
}