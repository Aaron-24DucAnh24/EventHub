namespace TicketBooking.API.Models
{
	public class Category
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public DateTime? UpdatedAt { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; }
		public ICollection<Event>? Events { get; set; }
	}
}