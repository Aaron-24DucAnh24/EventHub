using Microsoft.IdentityModel.Tokens;

namespace TicketBooking.API.Middlewares
{
  public class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _requestDelegate;

    public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
    {
      _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _requestDelegate(context);
      }
      catch (Exception exception)
      {
        int statusCode;
        string message;

        switch (exception)
        {
          case SecurityTokenExpiredException _:
            message = "Access Token expired";
            statusCode = 401;
            break;

          case SecurityTokenValidationException _:
            message = "Unauthorized or Invalid Access Token";
            statusCode = 401;
            break;

          default:
            message = exception.Message;
            statusCode = 500;
            break;
        }
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/text";
        await context.Response.WriteAsync(message);
      }
    }
  }
}