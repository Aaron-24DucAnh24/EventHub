using TicketBooking.API.Dtos;

namespace TicketBooking.API.Services
{
	public interface ICategoryService
	{
		public List<CategoryResponse> GetCategories();
  }
}