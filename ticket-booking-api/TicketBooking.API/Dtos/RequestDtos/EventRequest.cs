namespace TicketBooking.API.Dtos
{
  public class EventRequest
  {
    public IFormFile Image { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public DateTime Date { get; set; }
    public string StageName { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? City { get; set; }
    public List<string> Categories { get; set; } = new List<string>();
    public List<int> Prices { get; set; } = new List<int>();
  }
}