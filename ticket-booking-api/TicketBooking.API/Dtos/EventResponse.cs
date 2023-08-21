using TicketBooking.API.Enums;
using TicketBooking.API.Models;

namespace TicketBooking.API.Dtos
{
  public class EventResponse
  {
    public string Id { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int MinPrice { get; set; }
    public DateTime Date { get; set; }
    public string StageName { get; set; } = null!;
    public bool IsPublished { get; set; }
    public string Location { get; set; } = null!;
    public string? City { get; set; }
    public EventStatus Status { get; set; }
    public ICollection<CategoryResponse> Categories { get; set;} = new List<CategoryResponse>();
  }
}