using TicketBooking.API.Constants;

namespace TicketBooking.API.Services
{
  public class CookieService : ICookieService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public void ClearAccessToken()
    {
      SetCookie(CookieNames.ACCESS_TOKEN, string.Empty, -1);
    }

    public void ClearRefreshToken()
    {
      SetCookie(CookieNames.REFRESH_TOKEN, string.Empty, -1);
    }

    public void SetAccessToken(string token)
    {
      SetCookie(CookieNames.ACCESS_TOKEN, token, TokenSettings.ACCESS_TOKEN_EXPIRATION_MINUTES);
    }

    void ICookieService.SetRefreshToken(string token)
    {
      SetCookie(CookieNames.REFRESH_TOKEN, token, TokenSettings.REFRESH_TOKEN_EXPIRATIONS_MINUTES);
    }

    private void SetCookie(string key, string value, int expirationDays)
    {
      if(_httpContextAccessor.HttpContext == null)
        throw new Exception("Not found HttpContext");

      _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions()
      {
        Expires = DateTimeOffset.Now.AddMinutes(expirationDays)
      });
    }
  }
}