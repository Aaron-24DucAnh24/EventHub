using TicketBooking.API.Dtos;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface IUserRepository
  {
    public User? FindUser(string email);
    public Task<bool> CreateUserAsync(User user); 
  }
}