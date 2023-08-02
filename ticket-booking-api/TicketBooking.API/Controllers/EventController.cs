using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Services;
using TicketBooking.API.Dto;
using TicketBooking.API.Helper;
using AutoMapper;
using TicketBooking.API.Models;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class EventController: ControllerBase
	{
		private readonly IEventService _eventService;
		private readonly IMapper _mapper;

		public EventController(
			IEventService eventService,
			IMapper mapper)
		{
			_eventService = eventService;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<EventResponse>))]
		public ActionResult GetEvents([FromQuery] bool IsPublished)
		{
			var events = IsPublished
				? _mapper.Map<List<EventResponse>>(_eventService.GetPublishedEvents())
				: _mapper.Map<List<EventResponse>>(_eventService.GetUnPublishedEvents());	

			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(events);
		}

		[HttpGet("{eventId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<EventDetailResponse>))]
		[ProducesResponseType(400)]
		public ActionResult GetEvent(string eventId)
		{
			var e = _eventService.GetEventDetail(eventId);

			if(e == null){
				return NotFound();
			}

			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(_mapper.Map<EventDetailResponse>(e));
		}

		[HttpDelete("{eventId}")]
		[ProducesResponseType(200, Type = typeof(string))]
		[ProducesResponseType(400)]
		public ActionResult DeleteEvent(string eventId)
		{
			var e = _eventService.GetEvent(eventId);

			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (e == null)
			{
				return NotFound();
			}

			if(!_eventService.DeleteEvent(eventId))
			{
				return Problem(ResponseStatus.DeleteError);
			}

			return Ok(ResponseStatus.Success);
		}

		[HttpPut("{eventId}")]
		[ProducesResponseType(200, Type = typeof(string))]
		[ProducesResponseType(400)]
		public ActionResult SetPublished(string eventId)
		{
			Event? e = _eventService.GetEvent(eventId);

			if(e == null)
			{
				return NotFound();
			}

			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!_eventService.SetPublished(eventId))
			{
				return Problem(ResponseStatus.UpdateError);
			}

			return Ok(ResponseStatus.Success);
		}

		[HttpPost]
		[ProducesResponseType(204, Type = typeof(string))]
		[ProducesResponseType(400)]
		public async Task<ActionResult> CreateEvent([FromForm] EventRequest eventRequest)
		{
			if(
				eventRequest.Image.ContentType != "image/jpeg" 
				&& eventRequest.Image.ContentType != "image/png"
				&& eventRequest.Image.ContentType != "image/jpg")
				return BadRequest();

			var result = await _eventService.CreateEvent(eventRequest);

			if(!result)
			{
				ModelState.AddModelError("", ResponseStatus.AddError);
				return BadRequest(ModelState);
			}

			if(!ModelState.IsValid)
				return BadRequest();

			return Ok(ResponseStatus.Success);
		}
	}
}