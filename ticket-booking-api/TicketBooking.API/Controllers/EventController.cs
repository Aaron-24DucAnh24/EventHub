using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Services;
using TicketBooking.API.Dtos;
using TicketBooking.API.Helper;
using TicketBooking.API.Models;
using FluentValidation;
using FluentValidation.Results;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class EventController : ControllerBase
	{
		private readonly IEventService _eventService;
		private readonly ICacheService _cacheService;
		private readonly IValidator<EventRequest> _validator; 

		public EventController(
			IEventService eventService,
			ICacheService cacheService,
			IValidator<EventRequest> validator
		)
		{
			_eventService = eventService;
			_cacheService = cacheService;
			_validator = validator;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<EventResponse>))]
		public ActionResult GetEvents([FromQuery] bool IsPublished)
		{
			string cacheKey = IsPublished ? CacheKeys.PublishedEvents : CacheKeys.UnPublishedEvents;
			ICollection<EventResponse>? events = _cacheService.GetData<ICollection<EventResponse>>(cacheKey);

			if (events != null && events.Count > 0)
				return Ok(events);

			events = IsPublished
				? _eventService.GetPublishedEvents()
				: _eventService.GetUnPublishedEvents();

			_cacheService.SetData(cacheKey, events, DateTimeOffset.Now.AddMinutes(2));

			return Ok(events);
		}

		[HttpGet("{eventId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<EventDetailResponse>))]
		[ProducesResponseType(400)]
		public ActionResult GetEvent(string eventId)
		{
			EventDetailResponse? e = _eventService.GetEventDetail(eventId);

			if (e == null)
				return NotFound();

			return Ok(e);
		}

		[HttpDelete("{eventId}")]
		[ProducesResponseType(200, Type = typeof(string))]
		[ProducesResponseType(400)]
		public async Task<ActionResult> DeleteEvent(string eventId)
		{
			var e = _eventService.GetEvent(eventId);

			if (e == null)
			{
				return NotFound();
			}

			if (!await _eventService.DeleteEvent(eventId))
			{
				return Problem(ResponseStatus.DeleteError);
			}

			_cacheService.RemoveData(CacheKeys.PublishedEvents);
			_cacheService.RemoveData(CacheKeys.UnPublishedEvents);

			return Ok(ResponseStatus.Success);
		}

		[HttpPut("{eventId}")]
		[ProducesResponseType(200, Type = typeof(string))]
		[ProducesResponseType(400)]
		public ActionResult SetPublished(string eventId)
		{
			Event? e = _eventService.GetEvent(eventId);

			if (e == null)
			{
				return NotFound();
			}

			if (!_eventService.SetPublished(eventId))
			{
				return Problem(ResponseStatus.UpdateError);
			}

			_cacheService.RemoveData(CacheKeys.PublishedEvents);

			return Ok(ResponseStatus.Success);
		}

		[HttpPost]
		[ProducesResponseType(204, Type = typeof(string))]
		[ProducesResponseType(400)]
		public async Task<ActionResult> CreateEvent([FromForm] EventRequest eventRequest)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(eventRequest);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
				return BadRequest(ModelState);
			}

			var result = await _eventService.CreateEvent(eventRequest);

			if (!result)
			{
				ModelState.AddModelError("", ResponseStatus.AddError);
				return BadRequest(ModelState);
			}

			_cacheService.RemoveData(CacheKeys.UnPublishedEvents);

			return Ok(ResponseStatus.Success);
		}
	}
}