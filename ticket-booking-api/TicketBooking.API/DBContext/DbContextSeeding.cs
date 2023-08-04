using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TicketBooking.API.Models;

namespace TicketBooking.API.DBContext
{
  public class DbContextSeeding
  {
    public static void Run(ref ModelBuilder modelBuilder)
    {
      StreamReader categoryReader = new("./DBContext/SeedingData/Category.json");
      StreamReader eventReader = new("./DBContext/SeedingData/Event.json");
      StreamReader seatReader = new("./DBContext/SeedingData/Seat.json");
      StreamReader eventCategoryReader = new("./DBContext/SeedingData/EventCategory.json");
      StreamReader seatEventReader = new("./DBContext/SeedingData/SeatEvent.json");

      string categoriesJson = categoryReader.ReadToEnd();
      string eventJson = eventReader.ReadToEnd();
      string seatJson = seatReader.ReadToEnd();
      string eventCategoryJson = eventCategoryReader.ReadToEnd();
      string seatEventJson = seatEventReader.ReadToEnd();

      List<Category>? categories = JsonSerializer.Deserialize<List<Category>>(categoriesJson);
      List<Event>? events = JsonSerializer.Deserialize<List<Event>>(eventJson);
      List<Seat>? seats = JsonSerializer.Deserialize<List<Seat>>(seatJson);
      List<EventCategory>? eventCategories = JsonSerializer.Deserialize<List<EventCategory>>(eventCategoryJson);
      List<SeatEvent>? seatEvents = JsonSerializer.Deserialize<List<SeatEvent>>(seatEventJson);

      if(categories != null)
        foreach(var category in categories)
          modelBuilder.Entity<Category>().HasData(category);

      if(events != null)
        foreach(var e in events)
          modelBuilder.Entity<Event>().HasData(e);

      if(seats != null)
        foreach(var seat in seats)
          modelBuilder.Entity<Seat>().HasData(seat);

      if(seatEvents != null)
        foreach (var seatEvent in seatEvents)
          modelBuilder.Entity<SeatEvent>().HasData(seatEvent);

      if(eventCategories != null)
        foreach (var eventCategory in eventCategories)
          modelBuilder.Entity<EventCategory>().HasData(eventCategory);
    }
  }
}