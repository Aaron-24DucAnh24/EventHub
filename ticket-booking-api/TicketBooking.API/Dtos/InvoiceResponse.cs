namespace TicketBooking.API.Dtos
{
  public class InvoiceResponse
  {
    public string EventId { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public DateTime Date { get; set; }
    public string StageName { get; set; }
    public string Location { get; set; }
    public string Duration { get; set; }
		public string InvoiceId { get; set; }
    public List<SeatResponse> Seats { get; set; }
  }
}