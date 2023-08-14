using TicketBooking.API.DBContext;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public class UserConnectionRepository : IUserConnectionRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public UserConnectionRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<bool> CreateUserConnection(UserConnection userConnection)
    {
      await _dbContext.UserConnection.AddAsync(userConnection);
      return await _dbContext.SaveChangesAsync() != 0;
    }

    public async Task<bool> DeleteUserConnection(UserConnection userConnection)
    {
      _dbContext.UserConnection.Remove(userConnection);
      return await _dbContext.SaveChangesAsync() != 0;
    }

    public UserConnection? FindUserConnection(string refreshToken)
    {
      UserConnection? userConnection = _dbContext.UserConnection
        .Where(x => x.RefreshToken == refreshToken)
        .FirstOrDefault();

      return userConnection;
    }

    public async Task<bool> UpdateUserConnection(UserConnection userConnection)
    {
      _dbContext.UserConnection.Update(userConnection);
      return await _dbContext.SaveChangesAsync() != 0;
    }
  }
}