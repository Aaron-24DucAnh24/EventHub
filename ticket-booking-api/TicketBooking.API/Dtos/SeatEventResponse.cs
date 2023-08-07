using TicketBooking.API.Enum;

namespace TicketBooking.API.Dtos
{
  public class SeatEventResponse
  {
    public string SeatId { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public SeatStatus SeatStatus { get; set; }
    public int Price { get; set; }
    public SeatResponse Seat { get; set; } = null!;
  }
}