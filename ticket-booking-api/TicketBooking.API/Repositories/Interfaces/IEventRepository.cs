using TicketBooking.API.Dto;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface IEventRepository
  {
    ICollection<Event> GetUnPublishedEvents();
    ICollection<Event> GetPublishedEvents();
    Event? GetEvent(string id);
    bool AddEvent(Event e);
    bool UpdateEvent(Event e);
    bool DeleteEvent(string id);
    bool AddSeatEvent(EventRequest eventRequest, string eventId);
    bool AddEventCategory(List<string> categoryNames, string eventId);
  }
}