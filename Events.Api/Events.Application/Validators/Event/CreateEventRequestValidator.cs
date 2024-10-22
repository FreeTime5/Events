using Events.Application.Models.Event;
using FluentValidation;

namespace Events.Application.Validators.Event
{
    internal class CreateEventRequestValidator : AbstractValidator<CreateEventRequestDTO>
    {
        public CreateEventRequestValidator()
        {
            RuleFor(requestEvent => requestEvent.Title)
                .MaximumLength(100)
                .NotEmpty();
            RuleFor(requestEvent => requestEvent.Describtion)
                .MaximumLength(255);
            RuleFor(requestEvent => requestEvent.Image)
                .NotEmpty()
                .When(requestEvent => requestEvent.Image != null);
            RuleFor(requestEvent => requestEvent.MaxMembers)
                .GreaterThan(5)
                .WithMessage("Event must be at least with 5 members");
            RuleFor(requestEvent => requestEvent.Place)
                .NotEmpty();
            RuleFor(requestEvent => requestEvent.CategoryId)
                .NotEmpty()
                .When(requestEvent => requestEvent.CategoryId != null);
            RuleFor(requestEvent => requestEvent.Date)
                .GreaterThan(DateTime.UtcNow.AddDays(1))
                .WithMessage("Event must be at least 1 day later");
        }
    }
}
