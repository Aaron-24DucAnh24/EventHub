using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface ISeatRepository
  {
    public List<Seat> GetSeats();
    public List<SeatEvent> GetSeatEvents(List<string> seatId, string eventId);
    public SeatEvent? GetSeatEvent(string seatId, string eventId);
    public bool AddSeatInvoice(SeatInvoice seatInvoice);
    public bool UpdateSeatEvent(SeatEvent seatEvent);
  }
}