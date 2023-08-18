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
          case SecurityTokenValidationException _:
            message = "Unauthorized or access token expired";
            statusCode = 401;
            break;

          default:
            message = "Some unknown errors occurred";
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