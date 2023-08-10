using TicketBooking.API.Dtos;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface IUserRepository
  {
    public User? FindUser(LoginRequest request);
    public Task<bool> CreateUserAsync(RegisterRequest request); 
  }
}