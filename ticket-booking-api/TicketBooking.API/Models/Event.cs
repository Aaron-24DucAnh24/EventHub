using TicketBooking.API.Enum;

namespace TicketBooking.API.Models
{
  public class Event
  {
    public string Id { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int MinPrice { get; set; }
    public string Duration { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Location { get; set; } = null!;
    public string StageName { get; set; } = null!;
    public bool IsPublished { get; set; }
    public string? City { get; set; }
    public EventStatus Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Invoice> Invoices { get; set;} = new List<Invoice>();
    public ICollection<SeatEvent> SeatEvents{ get; set;} = new List<SeatEvent>();
    public ICollection<Category> Categories { get; set;} = new List<Category>();
  }
}