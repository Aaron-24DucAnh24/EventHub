namespace TicketBooking.API.Services
{
  public interface IBlobService
  {
    public Task<string> UpLoadImage(IFormFile file, string name);
  }
}