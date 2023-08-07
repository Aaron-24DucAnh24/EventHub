namespace TicketBooking.API.Models
{
	public class Invoice
	{
		public string Id { get; set; }  = null!;
		public string Name { get; set; }  = null!;
		public string Mail { get; set; }  = null!;
		public string Phone { get; set; }  = null!;
		public string Code { get; set; } = null!;
		public bool IsValidated { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; }
		public string EventId { get; set; } = null!;
		public ICollection<Seat> Seats { get; set; } = new List<Seat>();
		public Event Event { get; set; } = null!;
	}
}