using TicketBooking.API.Repository;
using TicketBooking.API.Dtos;
using AutoMapper;

namespace TicketBooking.API.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryService(
			ICategoryRepository categoryRepository,
			IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		public List<CategoryResponse> GetCategories()
		{
			return _mapper.Map<List<CategoryResponse>>(_categoryRepository.GetCategories());
		}
  }
}