namespace TicketBooking.API.Dtos
{
  public class InvoiceResponse
  {
    public string EventId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Image { get; set; } = null!;
    public DateTime Date { get; set; }
    public string StageName { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Duration { get; set; } = null!;
		public string InvoiceId { get; set; } = null!;
    public ICollection<SeatInInvoice> Seats { get; set; } = new List<SeatInInvoice>();
  }
}