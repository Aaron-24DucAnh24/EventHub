using TicketBooking.API.Enums;

namespace TicketBooking.API.Dtos
{
  public class SeatInInvoice
  {
    public string Name { get; set; } = null!;
    public SeatType Type { get; set; }
    public int Price { get; set; }
  }
}