using TicketBooking.API.Dtos;
using TicketBooking.API.Models;

namespace TicketBooking.API.Services
{
	public interface IEventService
	{
		public ICollection<Event> GetPublishedEvents();
		public ICollection<Event> GetUnPublishedEvents();
		public Event? GetEvent(string id);
		public Event? GetEventDetail(string eventId);
		public Task<bool> CreateEvent(EventRequest eventRequest);
		public bool SetPublished(string eventId);
		public Task<bool> DeleteEvent(string eventId);
  }
}