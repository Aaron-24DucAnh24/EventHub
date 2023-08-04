using FluentValidation;

namespace TicketBooking.API.Dtos.Validators
{
  public class InvoiceRequestValidator : AbstractValidator<InvoiceRequest>
  {
    public InvoiceRequestValidator()
    {
      RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required")
        .MaximumLength(255).WithMessage("Maximum length is 255");

      RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail is required")
        .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
        .WithMessage("Invalid mail format");

      RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");

      RuleFor(x => x.EventId).NotEmpty().WithMessage("EventId is required");

      RuleFor(x => x.seatIds).Must(s => s.Count > 0 && s.Count <= 3)
        .WithMessage("Valid number of seats is between 0 and 3");

      RuleForEach(x => x.seatIds).NotEmpty()
        .WithMessage("SeatId is required");
    }
  }
}