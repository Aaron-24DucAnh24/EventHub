using TicketBooking.API.Enum;

namespace TicketBooking.API.Dtos
{
  public class SeatResponse
  {
    public string Name { get; set; } = null!;
    public string Id { get; set; } = null!;
    public SeatType Type { get; set; }
  }
}