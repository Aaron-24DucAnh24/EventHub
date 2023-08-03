using TicketBooking.API.Dto;
using TicketBooking.API.Models;
using TicketBooking.API.Repository;

namespace TicketBooking.API.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;
		private readonly IBlobService _blobService;

		public EventService(
			IEventRepository eventRepository,
			IBlobService blobService)
		{
			_eventRepository = eventRepository;
			_blobService = blobService;
		}

		public ICollection<Event> GetPublishedEvents()
		{
			var result = _eventRepository.GetPublishedEvents();

			foreach (var e in result)
			{
				foreach (var category in e.Categories)
				{
					category.Events = null;
					category.UpdatedAt = null;
					category.CreatedAt = null;
					category.DeletedAt = null;
				}
			}

			return result;
		}

		public ICollection<Event> GetUnPublishedEvents()
		{
			var result = _eventRepository.GetUnPublishedEvents();

			foreach (var e in result)
			{
				foreach (var category in e.Categories)
				{
					category.Events = null;
					category.UpdatedAt = null;
					category.CreatedAt = null;
					category.DeletedAt = null;
				}
			}

			return result;
		}

		public Event? GetEventDetail(string eventId)
		{
			var result = _eventRepository.GetEvent(eventId);

			if (result == null)
				return result;

			foreach (var category in result.Categories)
			{
				category.Events = null;
				category.UpdatedAt = null;
				category.CreatedAt = null;
				category.DeletedAt = null;
			}

			foreach (var e in result.SeatEvents)
			{
				e.Event = null;
				e.Seat.SeatEvents = null;
				e.Seat.CreatedAt = null;
				e.Seat.UpdatedAt = null;
				e.Seat.DeletedAt = null;
				e.Seat.Invoices = null;
			}

			result.SeatEvents = result.SeatEvents
				.OrderBy(x => x.SeatId[0])
				.ThenBy(x => x.SeatId.Length)
				.ThenBy(x => x.SeatId)
				.ToList();

			return result;
		}

		public Event? GetEvent(string eventId)
		{
			return _eventRepository.GetEvent(eventId);
		}

		public async Task<bool> CreateEvent(EventRequest eventRequest)
		{
			var newEventId = Guid.NewGuid().ToString();

			string imgUrl = await _blobService.UpLoadImage(eventRequest.Image, newEventId);

			var newEvent = new Event
			{
				Id = newEventId,
				Duration = eventRequest.Duration,
				Title = eventRequest.Title,
				Image = imgUrl,
				MinPrice = eventRequest.Prices.Min(),
				StageName = eventRequest.StageName,
				City = eventRequest.City,
				Date = eventRequest.Date,
				CreatedAt = DateTime.Now,
				Location = eventRequest.Location,
			};

			if (!_eventRepository.AddEvent(newEvent))
				return false;

			if (!_eventRepository.AddSeatEvent(eventRequest, newEventId))
				return false;

			if (!_eventRepository.AddEventCategory(eventRequest.Categories, newEventId))
				return false;

			return true;
		}

		public async Task<bool> DeleteEvent(string eventId)
		{
			await _blobService.RemoveImage(eventId);
			return _eventRepository.DeleteEvent(eventId);
		}

		public bool SetPublished(string evenId)
		{
			var e = GetEvent(evenId);
			e.IsPublished = true;
			e.UpdatedAt = DateTime.Now;
			return _eventRepository.UpdateEvent(e);
		}
	}
}