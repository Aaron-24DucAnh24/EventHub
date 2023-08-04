using TicketBooking.API.Enum;

namespace TicketBooking.API.Dtos
{
  public class SeatInInvoice
  {
    public string Name { get; set; }
    public SeatType Type { get; set; }
    public int Price { get; set; }
  }
}