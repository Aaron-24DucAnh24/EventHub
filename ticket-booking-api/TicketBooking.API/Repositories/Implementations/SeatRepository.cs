using TicketBooking.API.DBContext;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public class SeatRepository : ISeatRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public SeatRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public List<Seat> GetSeats()
    {
      return _dbContext.Seats.ToList();
    }

    public bool AddSeatInvoice(SeatInvoice seatInvoice)
    {
      _dbContext.Add(seatInvoice);
      return _dbContext.SaveChanges() != 0;
    }

    public List<SeatEvent> GetSeatEvents(List<string> seatIds, string eventId)
    {
      return _dbContext.SeatEvents
        .Where(x => seatIds.Contains(x.SeatId))
        .Where(x => x.EventId == eventId)
        .ToList();
    }

    public bool UpdateSeatEvent(SeatEvent seatEvent)
    {
      _dbContext.Update(seatEvent);
      return _dbContext.SaveChanges() != 0;
    }

    public SeatEvent? GetSeatEvent(string seatId, string eventId)
    {
      return _dbContext.SeatEvents
        .Where(x => x.EventId == eventId)
        .Where(x => x.SeatId == seatId)
        .FirstOrDefault();
    }
  }
}