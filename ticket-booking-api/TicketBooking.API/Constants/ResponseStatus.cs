namespace TicketBooking.API.Constants
{
  public class ResponseStatus
  {
    static public string SUCCESS { get; } = "Success";
    static public string ADD_ERROR { get; } = "Something wrong while adding";
    static public string UPDATE_ERROR { get; } = "Something wrong while updating";
    static public string DELETE_ERROR { get; } = "Something wrong while deleting";
    static public string SERVICE_ERROR { get; } = "Something wrong while running service";
    static public string INVALID_REQUEST_PARAMETER { get; } = "Invalid request parameter";
    static public string AUTHENTICATION_ERROR { get; } = "Authentication information is incorrect";
  }
}