namespace TicketBooking.API.Dtos
{
	public class InvoiceRequest
	{
		public string FullName { get; set; } = null!;
		public string Mail { get; set; } = null!;
		public string Phone { get; set; } = null!;
		public string EventId { get; set; } = null!;
		public List<string> SeatIds { get; set; } = new List<string>();
	}
}