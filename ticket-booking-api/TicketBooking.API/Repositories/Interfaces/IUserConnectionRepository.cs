using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface IUserConnectionRepository
  {
    public Task<bool> CreateUserConnection(UserConnection userConnection);
    public Task<bool> DeleteUserConnection(UserConnection userConnection);
    public UserConnection? FindUserConnection(string refreshToken);
    public Task<bool> UpdateUserConnection(UserConnection userConnection);
  }
}