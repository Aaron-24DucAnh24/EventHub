using AutoMapper;
using TicketBooking.API.Models;

namespace TicketBooking.API.Dtos.MappingProfiles
{
  public class MappingProfiles: Profile
  {
    public MappingProfiles()
    {
      CreateMap<Event, EventResponse>();

      CreateMap<Event, EventDetailResponse>();

      CreateMap<Category, CategoryResponse>();

      CreateMap<SeatEvent, SeatEventResponse>();

      CreateMap<Seat, SeatResponse>();
    }
  }
}