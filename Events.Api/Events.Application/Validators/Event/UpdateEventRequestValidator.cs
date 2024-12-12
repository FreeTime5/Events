using Events.Application.Models.Event;
using FluentValidation;

namespace Events.Application.Validators.Event;

internal class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequestDTO>
{
    private const string GUIDREGEX = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$"; 

    public UpdateEventRequestValidator()
    {
        RuleFor(requestEvent => requestEvent.Id)
            .NotNull();
        RuleFor(requestEvent => requestEvent.Title)
            .MaximumLength(100)
            .NotEmpty()
            .When(e => e.Title != null);
        RuleFor(requestEvent => requestEvent.Describtion)
            .MaximumLength(255)
            .When(e => e.Describtion != null);
        RuleFor(requestEvent => requestEvent.Image)
            .NotEmpty()
            .When(e => e.Image != null);
        RuleFor(requestEvent => requestEvent.Place)
            .NotEmpty()
            .When(e => e.Place != null);
        RuleFor(requestEvent => requestEvent.CategoryId)
            .Matches(GUIDREGEX)
            .When(e => e.CategoryId != null);
        RuleFor(requestEvent => requestEvent.Date)
            .GreaterThan(DateTime.UtcNow.AddDays(1))
            .WithMessage("Event must be at least 1 day later")
            .When(e => e.Date != null);
    }
}
