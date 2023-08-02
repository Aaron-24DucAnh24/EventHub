using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface ICategoryRepository
  {
    public List<Category> GetCategories();
    public Category? GetCategory(string name);
  }
}