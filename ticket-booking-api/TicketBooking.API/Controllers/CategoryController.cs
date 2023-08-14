using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Dtos;
using TicketBooking.API.Services;
using TicketBooking.API.Constants;
using Microsoft.AspNetCore.Authorization;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly ICacheService _cacheService;

		public CategoryController(
			ICategoryService categoryService,
			ICacheService cacheService)
		{
			_categoryService = categoryService;
			_cacheService = cacheService;
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult GetCategories()
		{
			List<CategoryResponse>? result = _cacheService.GetData<List<CategoryResponse>>(CacheKeys.CATEGORIES);

			if (result != null && result.Count > 0)
				return Ok(result);

			result = _categoryService.GetCategories();

			_cacheService.SetData(CacheKeys.CATEGORIES, result, DateTimeOffset.Now.AddMinutes(2));

			return Ok(result);
		}
	}
}