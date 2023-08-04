using TicketBooking.API.Enum;
using TicketBooking.API.Models;

namespace TicketBooking.API.Dtos
{
  public class EventResponse
  {
    public string Id { get; set; }
    public string Image { get; set; }
    public string Title { get; set; }
    public int MinPrice { get; set; }
    public DateTime Date { get; set; }
    public string StageName { get; set; }
    public bool IsPublished { get; set; }
    public string Location { get; set; }
    public string? City { get; set; }
    public EventStatus Status { get; set; }
    public ICollection<CategoryResponse> Categories { get; set;}
  }
}