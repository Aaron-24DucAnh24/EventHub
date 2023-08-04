using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TicketBooking.API.Dtos;
using TicketBooking.API.Services;
using TicketBooking.API.Helper;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly ICacheService _cacheService;
		private readonly IMapper _mapper;

		public CategoryController(
			ICategoryService categoryService,
			ICacheService cacheService,
			IMapper mapper)
		{
			_categoryService = categoryService;
			_cacheService = cacheService;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(204, Type = typeof(IEnumerable<CategoryResponse>))]
		public ActionResult GetCategories()
		{
			List<CategoryResponse>? result = _cacheService.GetData<List<CategoryResponse>>(CacheKeys.Categories);

			if(result != null && result.Count > 0)
				return Ok(result);

			result = _mapper.Map<List<CategoryResponse>>(_categoryService.GetCategories());

			_cacheService.SetData(CacheKeys.Categories, result, DateTimeOffset.Now.AddMinutes(2));

			return Ok(result);
		}
	}
}