using TicketBooking.API.DBContext;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public class CategoryRepository : ICategoryRepository
  {
    private readonly ApplicationDbContext _dbContext;
    
    public CategoryRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public List<Category> GetCategories()
    {
      var result = _dbContext.Categories.Where(c => !c.IsDeleted).ToList();
      return result;
    }

    public Category? GetCategory(string name)
    {
      return _dbContext.Categories.Where(c => c.Name == name).FirstOrDefault();
    }
  }
}