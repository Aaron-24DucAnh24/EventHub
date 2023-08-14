namespace TicketBooking.API.Services
{
  public interface IBlobService
  {
    public Task<string> UpLoadImageAsync(IFormFile file, string name);
    public Task<bool> RemoveImageAsync(string blogName);
  }
}