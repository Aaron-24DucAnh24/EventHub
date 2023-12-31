using FluentValidation;

namespace TicketBooking.API.Dtos.Validators
{
  public class LoginRequestValidator : AbstractValidator<LoginRequest>
  {
    public LoginRequestValidator()
    {
      RuleFor(x => x.Email).NotEmpty().WithMessage("Mail is required")
        .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
        .WithMessage("Invalid mail format");
      
      RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
  }
}