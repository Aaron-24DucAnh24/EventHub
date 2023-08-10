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

    public async Task<bool> CreateUserAsync(RegisterRequest request)
    {
      User user = new()
      {
        Id = Guid.NewGuid().ToString(),
        Name = request.Name,
        Email = HashHelper.GetHash(request.Email),
        Password = HashHelper.GetHash(request.Password)
      };
      await _dbContext.AddAsync(user);
      return await _dbContext.SaveChangesAsync() != 0;
    }

    public User? FindUser(LoginRequest request)
    {
      User? user = _dbContext.Users
        .Where(x => HashHelper.CompareHash(x.Email, request.Email))
        .Where(x => HashHelper.CompareHash(x.Password, request.Password))
        .FirstOrDefault();

      return user;
    }
  }
}