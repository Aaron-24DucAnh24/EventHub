using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
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

    public AuthService(
      IUserRepository userRepository,
      IUserConnectionRepository userConnectionRepository)
    {
      _userRepository = userRepository;
      _userConnectionRepository = userConnectionRepository;
    }

    public async Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request)
    {
      if (_userRepository.FindUser(request.Email) != null)
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

      UserConnection userConnection = new()
      {
        Email = request.Email,
        Password = HashHelper.GetHash(request.Password)
      };

      if(!await _userConnectionRepository.CreateUserConnectionAsync(userConnection))
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
      if (accessToken == null)
        return null;

      string refreshToken = TokenHelper.GenerateRefreshToken(user);

      UserConnection? userConnection = _userConnectionRepository.FindUserConnectionByEmail(request.Email);
      if(userConnection == null)
        return null;

      userConnection.AccessToken = accessToken;
      userConnection.RefreshToken = refreshToken;
      userConnection.RefreshTokenExpiredDate = DateTimeOffset.Now.AddDays(7);
      userConnection.AccessTokenExpiredDate = DateTimeOffset.Now.AddMinutes(15);

      AuthenticationResponse response = new()
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };

      if (await _userConnectionRepository.UpdateUserConnectionAsync(userConnection))
        return response;

      return null;
    }

    public async Task<string?> RefreshTokenAsync(string token)
    {
      UserConnection? userConnection = _userConnectionRepository.FindUserConnectionByRefreshToken(token);
      if (userConnection == null)
        return null;

      User? user = _userRepository.FindUser(userConnection.Email);
      if (user == null)
        return null;

      string? accessToken = TokenHelper.GenerateAccessToken(user);
      if (accessToken == null)
        return null;

      userConnection.AccessToken = accessToken;
      userConnection.AccessTokenExpiredDate = DateTime.Now.AddMinutes(15);
      if (await _userConnectionRepository.UpdateUserConnectionAsync(userConnection))
        return accessToken;

      return null;
    }

    public bool ValidateAccessToken(string token)
    {
      SecurityToken? securityToken = null;
      JwtSecurityTokenHandler tokenHandler = new();

      try
      {
        tokenHandler.ValidateToken(token, TokenHelper.CreateTokenValidationParameters(), out securityToken);
      }
      catch (Exception)
      {
        throw new SecurityTokenExpiredException(); 
      }

      if(securityToken == null)
      {
        return false;
      }

      JwtSecurityToken validatedToken = (JwtSecurityToken) securityToken;
      string? email =  validatedToken.GetEmail();

      if(email == null)
      {
        return false;
      } 

      UserConnection? userConnection = _userConnectionRepository.FindUserConnectionByEmail(email);

      if(userConnection == null)
      {
        return false;
      }

      if(userConnection.AccessTokenExpiredDate < DateTimeOffset.Now)
      {
        return false;
      }

      return true;
    }
  }
}