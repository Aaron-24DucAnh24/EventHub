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

    public Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request)
    {
      throw new NotImplementedException();
    }

		public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
		{
			//User? user = _userRepository.FindUser(request);

			//if (user == null)
			//  return null;

			throw new NotImplementedException();
		}

		public async Task<string> RefreshTokenAsync(string token)
    {
      throw new NotImplementedException();
    }
	}
}