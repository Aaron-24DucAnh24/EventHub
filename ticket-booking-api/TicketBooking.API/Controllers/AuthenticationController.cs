using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Dtos;
using TicketBooking.API.Services;
using TicketBooking.API.Helper;
using TicketBooking.API.Constants;

namespace TicketBooking.API.Controller
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthenticationController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly IValidator<RegisterRequest> _registerRequestValidator;

    public AuthenticationController(
      IAuthService authService,
      IValidator<LoginRequest> loginRequestValidator,
      IValidator<RegisterRequest> registerRequestValidator)
    {
      _authService = authService;
      _loginRequestValidator = loginRequestValidator;
      _registerRequestValidator = registerRequestValidator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync([FromForm] RegisterRequest request)
    {
      ValidationResult validationResult = await _registerRequestValidator.ValidateAsync(request);

      if(!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
        return BadRequest(ModelState);
      }

      AuthenticationResponse? result = await _authService.RegisterAsync(request);

      if(result == null)
      {
        ModelState.AddModelError("", ResponseStatus.AUTHENTICATION_ERROR);
        return BadRequest(ModelState);
      }

      return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> LoginAsync([FromForm] LoginRequest request)
    {
      ValidationResult validationResult = await _loginRequestValidator.ValidateAsync(request);

      if(!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
        return BadRequest(ModelState);
      }

      AuthenticationResponse? result = await _authService.LoginAsync(request);

      if(result == null)
      {
        ModelState.AddModelError("", ResponseStatus.AUTHENTICATION_ERROR);
        return BadRequest(ModelState);
      }

      return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RefreshTokenAsync([FromBody] string refreshToken)
    {
      return Ok("string");
    }
  }
}