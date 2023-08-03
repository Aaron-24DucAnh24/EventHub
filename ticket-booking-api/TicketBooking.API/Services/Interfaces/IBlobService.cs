namespace TicketBooking.API.Services
{
  public interface IBlobService
  {
    public Task<string> UpLoadImage(IFormFile file, string name);
    public Task<bool> RemoveImage(string blogName);
  }
}