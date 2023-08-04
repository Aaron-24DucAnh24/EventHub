using TicketBooking.API.Enum;

namespace TicketBooking.API.Dtos
{
  public class SeatEventResponse
  {
    public string SeatId { get; set; }
    public string EventId { get; set; }
    public SeatStatus SeatStatus { get; set; }
    public int Price { get; set; }
    public SeatResponse Seat { get; set; }
  }
}