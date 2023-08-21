namespace TicketBooking.API.Services
{
  public interface ICookieService
  {
    public void SetAccessToken(string token);
    public void ClearAccessToken();
    public void SetRefreshToken(string token);
    public void ClearRefreshToken();
  }
}