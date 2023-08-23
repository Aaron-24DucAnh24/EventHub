using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface IUserConnectionRepository
  {
    public Task<bool> CreateUserConnectionAsync(UserConnection userConnection);
    public Task<bool> DeleteUserConnectionAsync(UserConnection userConnection);
    public UserConnection? FindUserConnectionByRefreshToken(string refreshToken);
    public UserConnection? FindUserConnectionByEmail(string email);
    public Task<bool> UpdateUserConnectionAsync(UserConnection userConnection);
  }
}