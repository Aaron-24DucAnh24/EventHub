using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        signingCredentials: creds
      );

      JwtSecurityTokenHandler tokenHandler= new();
      return tokenHandler.WriteToken(token);
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

    public static TokenValidationParameters CreateTokenValidationParameters()
    {
      string? issuer = ConfigurationHelper.configuration.GetValue<string>("Token:Issuer");
      string? signingKey = ConfigurationHelper.configuration.GetValue<string>("Token:Key");

      if (signingKey == null || issuer == null)
      {
        throw new ArgumentNullException("CreateTokenValidation arguments cannot be null");
      }

      byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);

      return new TokenValidationParameters()
      {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
      };
    }

    public static string? GetAccessTokenFromRequest(HttpRequest request)
    {
      string? bearerToken = request.Headers["Authorization"].FirstOrDefault();
      if(bearerToken == null)
      {
        return null;
      }

      string jwtToken = bearerToken.Substring($"{JwtBearerDefaults.AuthenticationScheme} ".Length).Trim();
      return jwtToken;
    }
  }
}