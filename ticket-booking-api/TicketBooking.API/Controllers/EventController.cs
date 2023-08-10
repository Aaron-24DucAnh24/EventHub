using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Services;
using TicketBooking.API.Dtos;
using TicketBooking.API.Helper;
using TicketBooking.API.Models;
using TicketBooking.API.Constants;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

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
		[AllowAnonymous]
		public ActionResult GetEvents([FromQuery] bool IsPublished)
		{
			string cacheKey = IsPublished ? CacheKeys.PUBLISHED_EVENTS : CacheKeys.UNPUBLISHED_EVENTS;
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
		[AllowAnonymous]
		public ActionResult GetEvent(string eventId)
		{
			EventDetailResponse? e = _eventService.GetEventDetail(eventId);

			if (e == null)
				return NotFound();

			return Ok(e);
		}

		[HttpDelete("{eventId}")]
		[AllowAnonymous]
		public async Task<ActionResult> DeleteEventAsync(string eventId)
		{
			var e = _eventService.GetEvent(eventId);

			if (e == null)
			{
				return NotFound();
			}

			if (!await _eventService.DeleteEventAsync(e))
			{
				return Problem(ResponseStatus.DELETE_ERROR);
			}

			_cacheService.RemoveData(CacheKeys.PUBLISHED_EVENTS);
			_cacheService.RemoveData(CacheKeys.UNPUBLISHED_EVENTS);

			return Ok(ResponseStatus.SUCCESS);
		}

		[HttpPut("{eventId}")]
		[AllowAnonymous]
		public ActionResult SetPublished(string eventId)
		{
			Event? e = _eventService.GetEvent(eventId);

			if (e == null)
			{
				return NotFound();
			}

			if (!_eventService.SetPublished(e))
			{
				return Problem(ResponseStatus.UPDATE_ERROR);
			}

			_cacheService.RemoveData(CacheKeys.PUBLISHED_EVENTS);

			return Ok(ResponseStatus.SUCCESS);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> CreateEventAsync([FromForm] EventRequest eventRequest)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(eventRequest);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
				return BadRequest(ModelState);
			}

			var result = await _eventService.CreateEventAsync(eventRequest);

			if (!result)
			{
				ModelState.AddModelError("", ResponseStatus.ADD_ERROR);
				return BadRequest(ModelState);
			}

			_cacheService.RemoveData(CacheKeys.UNPUBLISHED_EVENTS);

			return Ok(ResponseStatus.SUCCESS);
		}
	}
}