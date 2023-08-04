namespace TicketBooking.API.Helper
{
  public class ConfigurationString
  {
    static private IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    static public readonly string Database = configuration.GetConnectionString("TicketBookingDatabase");

    static public readonly string BlobStorage = configuration.GetConnectionString("TicketBookingStorage");

    static public readonly string SmtpClient = configuration.GetConnectionString("SmtpClient");

    static public readonly string EmailClient = configuration.GetConnectionString("EmailClient");
    
    static public readonly string EmailPassword = configuration.GetConnectionString("Password");
  }
}