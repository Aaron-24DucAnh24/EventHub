namespace TicketBooking.API.Constants
{
  public class ResponseMessage
  {
    static public readonly string SUCCESS  = "Success";

    static public readonly string ADD_ERROR  = "Something wrong while adding";

    static public readonly string UPDATE_ERROR  = "Something wrong while updating";

    static public readonly string DELETE_ERROR  = "Something wrong while deleting";

    static public readonly string SERVICE_ERROR  = "Something wrong while running service";

    static public readonly string INVALID_REQUEST_PARAMETER  = "Invalid request parameter";

    static public readonly string AUTHENTICATION_INCORRECT = "Authentication information is incorrect";
    
    static public readonly string AUTHENTICATION_DUPLICATE = "Duplicated account";
  }
}