using TicketBooking.API.Repository;
using TicketBooking.API.Models;

namespace TicketBooking.API.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public List<Category> GetCategories()
		{
			return _categoryRepository.GetCategories();
		}
  }
}