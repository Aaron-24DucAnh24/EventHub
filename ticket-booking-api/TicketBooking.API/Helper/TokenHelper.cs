using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TicketBooking.API.Models;

namespace TicketBooking.API.Helper
{
  public class TokenHelper
  {
    public static string? GenerateAccessToken(User user)
    {
      List<Claim> claims = new()
      {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role.ToString())
      };

      string? keyConfig = ConfigurationHelper.configuration.GetValue<string>("Token:Key");
      string? issuerConfig = ConfigurationHelper.configuration.GetValue<string>("Token:Issuer");
      if (keyConfig == null || issuerConfig == null)
        return null;

      SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(keyConfig));
      SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
      JwtSecurityToken token = new(
        issuerConfig,
        issuerConfig,
        claims,
        expires: DateTime.Now.AddHours(3),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateRefreshToken(User user)
    {
      Random random = new();
      int length = 16;
      string token = "";
      for (var i = 0; i < length; i++)
      {
        token += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
      }

      return token;
    }
  }
}