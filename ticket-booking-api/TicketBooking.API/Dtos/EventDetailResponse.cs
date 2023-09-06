using TicketBooking.API.Models;
using TicketBooking.API.Enums;

namespace TicketBooking.API.Dtos
{
  public class EventDetailResponse
  {
    public string Id { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string StageName { get; set; } = null!;
    public int MinPrice { get; set; }
    public bool IsPublished { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = null!;
    public string? City { get; set; }
    public EventStatus Status { get; set; }
    public string Duration { get; set; } = null!;
    public ICollection<CategoryResponse> Categories { get; set;} = new List<CategoryResponse>();
    public ICollection<SeatEventResponse> SeatEvents{ get; set;} = new List<SeatEventResponse>();
  }
}