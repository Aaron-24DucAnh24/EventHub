using TicketBooking.API.Enums;

namespace TicketBooking.API.Models
{
	public class SeatEvent
	{
		public string SeatId { get; set; } = null!;
		public string EventId { get; set; } = null!;
		public SeatStatus SeatStatus { get; set; }
		public int Price { get; set; }
		public Event Event { get; set; } = null!;
		public Seat Seat { get; set; } = null!;
	}
}