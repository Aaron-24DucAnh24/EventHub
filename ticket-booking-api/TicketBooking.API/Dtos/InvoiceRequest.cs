namespace TicketBooking.API.Dtos
{
	public class InvoiceRequest
	{
		public string FullName { get; set; }
		public string Mail { get; set; }
		public string Phone { get; set; }
		public string EventId { get; set; }
		public List<string> seatIds { get; set; }
	}
}