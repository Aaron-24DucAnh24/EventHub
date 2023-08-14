using Microsoft.EntityFrameworkCore;
using TicketBooking.API.DBContext;
using TicketBooking.API.Dtos;
using TicketBooking.API.Enum;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public class EventRepository : IEventRepository
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISeatRepository _seatRepository;

    public EventRepository(
      ApplicationDbContext dbContext,
      ICategoryRepository categoryRepository,
      ISeatRepository seatRepository)
    {
      _dbContext = dbContext;
      _categoryRepository = categoryRepository;
      _seatRepository = seatRepository;
    }

    public bool AddEvent(Event e)
    {
      _dbContext.Add(e);
      return _dbContext.SaveChanges() != 0;
    }

    public bool DeleteEvent(string id)
    {
      Event? e = GetEvent(id);

      if (e == null)
        return false;

      e.IsDeleted = true;
      return UpdateEvent(e);
    }

    public Event? GetEvent(string id)
    {
      Event? result = _dbContext.Events
        .Where(x => (x.Id == id) && (!x.IsDeleted))
        .Include(x => x.SeatEvents)
        .ThenInclude(x => x.Seat)
        .Include(x => x.Categories)
        .FirstOrDefault();

      return result;
    }

    public ICollection<Event> GetPublishedEvents()
    {
      ICollection<Event> result = _dbContext.Events
        .Where(x => !x.IsDeleted && x.IsPublished)
        .Include(x => x.Categories)
        .OrderBy(x => x.Date)
        .ToList();

      return result;
    }

    public ICollection<Event> GetUnPublishedEvents()
    {
      ICollection<Event> result = _dbContext.Events
        .Where(x => !x.IsDeleted && !x.IsPublished)
        .Include(x => x.Categories)
        .ToList();

      return result;
    }

    public bool UpdateEvent(Event e)
    {
      _dbContext.Update(e);
      return _dbContext.SaveChanges() != 0;
    }

    bool IEventRepository.AddEventCategory(List<string> categoryNames, string eventId)
    {
      foreach (var categoryName in categoryNames)
      {
        Category? category = _categoryRepository.GetCategory(categoryName);

        if(category == null)
          return false;

        EventCategory eventCategory = new()
        {
          CategoryId = category.Id,
          EventId = eventId,
        };

        _dbContext.Add(eventCategory);

        if(_dbContext.SaveChanges() == 0)
          return false;
      }

      return true;
    }

    bool IEventRepository.AddSeatEvent(EventRequest eventRequest, string eventId)
    {
      List<Seat> seats = _seatRepository.GetSeats();

      foreach (var seat in seats)
      {
        SeatEvent seatEvent = new()
        {
          SeatId = seat.Id,
          EventId = eventId,
          SeatStatus = seat.Name == "F18" || seat.Name == "F1"
            ? SeatStatus.Banned
            : SeatStatus.Free,
          Price = eventRequest.Prices[(int)seat.Type]
        };
        _dbContext.Add(seatEvent);
      }

      return _dbContext.SaveChanges() != 0;
    }
  }
}