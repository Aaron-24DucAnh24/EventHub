using AutoMapper;
using TicketBooking.API.Dtos;
using TicketBooking.API.Models;
using TicketBooking.API.Repository;

namespace TicketBooking.API.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;
		private readonly IBlobService _blobService;
		private readonly IMapper _mapper;

		public EventService(
			IEventRepository eventRepository,
			IBlobService blobService,
			IMapper mapper
		)
		{
			_eventRepository = eventRepository;
			_blobService = blobService;
			_mapper = mapper;
		}

		public ICollection<EventResponse> GetPublishedEvents()
		{
			ICollection<Event> events = _eventRepository.GetPublishedEvents();
			ICollection<EventResponse> result = new List<EventResponse>();

			foreach (var e in events)
			{
				EventResponse eventResponse = _mapper.Map<EventResponse>(e);
				result.Add(eventResponse);
			}

			return result;
		}

		public ICollection<EventResponse> GetUnPublishedEvents()
		{
			ICollection<Event> events = _eventRepository.GetUnPublishedEvents();
			ICollection<EventResponse> result = new List<EventResponse>();

			foreach (var e in events)
			{
				EventResponse eventResponse = _mapper.Map<EventResponse>(e);
				result.Add(eventResponse);
			}

			return result;
		}

		public EventDetailResponse? GetEventDetail(string eventId)
		{
			Event? e = _eventRepository.GetEvent(eventId);

			if (e == null)
				return null;

			EventDetailResponse result = _mapper.Map<EventDetailResponse>(e);

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