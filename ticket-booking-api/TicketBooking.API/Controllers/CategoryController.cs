using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TicketBooking.API.Dto;
using TicketBooking.API.Services;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryService categoryService, IMapper mapper)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(204, Type = typeof(IEnumerable<CategoryResponse>))]
		public ActionResult GetCategories()
		{
			var result = _mapper
				.Map<List<CategoryResponse>>(_categoryService.GetCategories());

			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(result);
		}

	}
}