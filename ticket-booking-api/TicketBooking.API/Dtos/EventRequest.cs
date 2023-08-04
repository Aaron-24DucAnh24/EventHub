namespace TicketBooking.API.Dtos
{
  public class EventRequest
  {
    public IFormFile Image { get; set; }
    public string Title { get; set; }
    public string Duration { get; set; }
    public DateTime Date { get; set; }
    public string StageName { get; set; }
    public string Location { get; set; }
    public string? City { get; set; }
    public List<string> Categories { get; set; }
    public List<int> Prices { get; set; }
  }
}