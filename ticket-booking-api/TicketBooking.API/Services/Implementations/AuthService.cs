using System.Security.Claims;
using TicketBooking.API.Dtos;
using TicketBooking.API.Helper;
using TicketBooking.API.Models;
using TicketBooking.API.Repository;

namespace TicketBooking.API.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _userRepository;
    private readonly IUserConnectionRepository _userConnectionRepository; 
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
      IUserRepository userRepository,
      IUserConnectionRepository userConnectionRepository,
      IHttpContextAccessor httpContextAccessor)
    {
      _userRepository = userRepository;
      _userConnectionRepository = userConnectionRepository;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request)
    {
      if(_userRepository.FindUser(request.Email) != null)
        return null;

      User user = new()
      {
        Email = request.Email,
        Name = request.Name,
        Password = request.Password,
        Id = Guid.NewGuid().ToString()
      };

      if(!await _userRepository.CreateUserAsync(user))
        return null;

      return await LoginAsync(new LoginRequest()
      {
        Email = request.Email,
        Password = request.Password,
      });
    }

		public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
		{
      User? user = _userRepository.FindUser(request.Email);      
	    if (user == null || !HashHelper.CompareHash(request.Password, user.Password))
			 return null;

      string? accessToken = TokenHelper.GenerateAccessToken(user);
      if(accessToken == null)
        return null;

      string refreshToken = TokenHelper.GenerateRefreshToken(user);

      AuthenticationResponse response = new()
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };

      UserConnection userConnection = new()
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        Email =request.Email,
        Password = HashHelper.GetHash(request.Password)
      };

      if(await _userConnectionRepository.CreateUserConnection(userConnection))
        return response;

      return null;
    }

		public async Task<string?> RefreshTokenAsync(string token)
    {
      UserConnection? userConnection = _userConnectionRepository.FindUserConnection(token);
      if(userConnection == null) 
        return null;

      User? user = _userRepository.FindUser(userConnection.Email);
      if(user == null)
        return null;

      string? accessToken = TokenHelper.GenerateAccessToken(user);
      if(accessToken == null)
        return null;

      userConnection.AccessToken = accessToken;
      if(await _userConnectionRepository.UpdateUserConnection(userConnection))
        return accessToken;

      return null;
    }
	}
}