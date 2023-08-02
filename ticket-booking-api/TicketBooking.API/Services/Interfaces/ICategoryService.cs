using TicketBooking.API.Models;

namespace TicketBooking.API.Services
{
	public interface ICategoryService
	{
		public List<Category> GetCategories();
  }
}