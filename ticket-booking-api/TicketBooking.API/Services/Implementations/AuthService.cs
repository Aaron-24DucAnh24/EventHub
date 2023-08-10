using TicketBooking.API.Dtos;
using TicketBooking.API.Models;
using TicketBooking.API.Repository;

namespace TicketBooking.API.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    // public async AuthenticationResponse? LoginAsync(LoginRequest request)
    // {
    //   User? user = _userRepository.FindUser(request);

    //   if (user == null)
    //     return null;
    // }

    public async Task<string> RefreshTokenAsync(string token)
    {
      throw new NotImplementedException();
    }

    public Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request)
    {
      throw new NotImplementedException();
    }

    Task<AuthenticationResponse?> IAuthService.LoginAsync(LoginRequest request)
    {
      throw new NotImplementedException();
    }
  }
}