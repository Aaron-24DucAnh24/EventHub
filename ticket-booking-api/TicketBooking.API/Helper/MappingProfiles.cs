using AutoMapper;
using TicketBooking.API.Dtos;
using TicketBooking.API.Models;

namespace TicketBooking.API.Helper
{
  public class MappingProfiles: Profile
  {
    public MappingProfiles()
    {
      CreateMap<Event, EventResponse>();

      CreateMap<Event, EventDetailResponse>();

      CreateMap<Category, CategoryResponse>();
      
      CreateMap<Invoice, InvoiceRequest>();
    }
  }
}