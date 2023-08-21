using TicketBooking.API.Dtos;

namespace TicketBooking.API.Services
{
  public interface IAuthService
  { 
    public Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request);
    public Task<AuthenticationResponse?> LoginAsync(LoginRequest request);
    public Task<bool> LogoutAsync();
    public bool ValidateAccessToken(string token);
    public Task<string?> RefreshTokenAsync(string token);
  }
}