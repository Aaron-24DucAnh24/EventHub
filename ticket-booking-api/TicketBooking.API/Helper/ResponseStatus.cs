namespace TicketBooking.API.Helper
{
  public class ResponseStatus
  {
    static public string Success { get; } = "Success";
    static public string AddError { get; } = "Something wrong while adding";
    static public string UpdateError { get; } = "Something wrong while updating";
    static public string DeleteError { get; } = "Something wrong while deleting";
    static public string ServiceError { get; } = "Something wrong while running service";
    static public string InvalidRequestParam { get; } = "Invalid request parameter";
  }
}