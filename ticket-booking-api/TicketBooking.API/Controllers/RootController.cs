using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicketBooking.API.Controller
{
  [ApiController]
  [AllowAnonymous]
  [Route("/")]
  public class RootController : ControllerBase
  {
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(string))]
    public ActionResult<string> Get()
    {
      return "Welcome to API service for TicketBooking!";
    }
  }
}