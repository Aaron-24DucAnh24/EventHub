using TicketBooking.API.Enums;

namespace TicketBooking.API.Models
{
	public class Seat
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; }  = null!;
		public SeatType Type { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; }
		public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
		public ICollection<SeatEvent> SeatEvents { get; set; } = new List<SeatEvent>();
	}
}