using TicketBooking.API.Models;
using TicketBooking.API.Enum;

namespace TicketBooking.API.Dtos
{
  public class EventDetailResponse
  {
    public string Id { get; set; }
    public string Image { get; set; }
    public string Title { get; set; }
    public string StageName { get; set; }
    public int MinPrice { get; set; }
    public bool IsPublished { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string? City { get; set; }
    public EventStatus Status { get; set; }
    public string Duration { get; set; }
    public ICollection<Category> Categories { get; set;}
    public ICollection<SeatEvent> SeatEvents{ get; set;} 
  }
}