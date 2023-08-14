using FluentValidation;

namespace TicketBooking.API.Dtos.Validators
{
  public class EventRequestValidator : AbstractValidator<EventRequest>
  {
    public EventRequestValidator()
    {
      RuleFor(x => x.Image).NotNull()
        .WithMessage("Image is required")
        .Must(i => i.ContentType.Contains("image"))
        .WithMessage("Unsupported image type");

      RuleFor(x => x.Title).NotEmpty()
        .WithMessage("Title is required");

      RuleFor(x => x.Duration).NotEmpty()
        .WithMessage("Duration is required");

      RuleFor(x => x.Date).GreaterThan(DateTime.Now)
        .WithMessage("Date must be a future date-time");

      RuleFor(x => x.StageName).NotEmpty()
        .WithMessage("StageName is required");

      RuleFor(x => x.Location).NotEmpty()
        .WithMessage("Location is required");

      RuleFor(x => x.Categories).Must(c => c.Count >= 1)
        .WithMessage("Require at least one category.");

      RuleFor(x => x.Prices).Must(p => p.Count == 3)
        .WithMessage("Require three prices");

      RuleForEach(x => x.Prices).GreaterThan(0)
        .WithMessage("Price must be greater then zero");
    }
  }
}