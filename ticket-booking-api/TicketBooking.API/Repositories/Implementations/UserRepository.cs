using TicketBooking.API.DBContext;
using TicketBooking.API.Dtos;
using TicketBooking.API.Helper;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
      await _dbContext.AddAsync(user);
      return await _dbContext.SaveChangesAsync() != 0;
    }

    public User? FindUser(string email)
    {
      User? user = _dbContext.Users
        .Where(x => x.Email == email)
        .FirstOrDefault();

      return user;
    }
  }
}