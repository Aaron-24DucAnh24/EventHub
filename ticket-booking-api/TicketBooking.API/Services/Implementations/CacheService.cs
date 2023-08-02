using System.Runtime.Caching;

namespace TicketBooking.API.Services
{
  public class CacheService : ICacheService
  {
    private readonly ObjectCache _memoryCache = MemoryCache.Default;

    public T GetData<T>(string key)
    {
      throw new NotImplementedException();
    }

    public object RemoveData(string key)
    {
      throw new NotImplementedException();
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
      throw new NotImplementedException();
    }
  }
}