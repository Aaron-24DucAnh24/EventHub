namespace TicketBooking.API.Models
{
  public class UserConnection
  {
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime RefreshTokenExpiredDate { get; set; }
    public bool IsDeleted { get; set; }
  }
}