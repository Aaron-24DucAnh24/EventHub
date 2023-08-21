using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Dtos;
using TicketBooking.API.Services;
using TicketBooking.API.Extensions;
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
    private readonly ICookieService _cookieService;

    public AuthenticationController(
      IAuthService authService,
      IValidator<LoginRequest> loginRequestValidator,
      IValidator<RegisterRequest> registerRequestValidator,
      ICookieService cookieService)
    {
      _authService = authService;
      _loginRequestValidator = loginRequestValidator;
      _registerRequestValidator = registerRequestValidator;
      _cookieService = cookieService;
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
        ModelState.AddModelError("", ResponseMessage.AUTHENTICATION_DUPLICATE);
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
        ModelState.AddModelError("", ResponseMessage.AUTHENTICATION_INCORRECT);
        return BadRequest(ModelState);
      }

      _cookieService.SetAccessToken(result.AccessToken);
      _cookieService.SetRefreshToken (result.RefreshToken);

      return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RefreshTokenAsync([FromBody] string refreshToken)
    {
      string? accessToken = await _authService.RefreshTokenAsync(refreshToken);
      if(accessToken != null)
        _cookieService.SetAccessToken(accessToken);

      return Ok(accessToken);
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<ActionResult<string>> LogoutAsync()
    {
      await _authService.LogoutAsync();
      _cookieService.ClearAccessToken();
      _cookieService.ClearRefreshToken();
      return Ok(ResponseMessage.SUCCESS);
    }

    [HttpGet("test-authentication")]
    [Authorize]
    public ActionResult<string> TestAuthentication()
    {
      return Ok(ResponseMessage.SUCCESS);
    }
  }
}