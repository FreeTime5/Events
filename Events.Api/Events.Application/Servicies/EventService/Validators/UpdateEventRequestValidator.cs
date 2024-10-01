using Events.Application.Servicies.EventService.DTOs;
using FluentValidation;

namespace Events.Application.Servicies.EventService.Validators;

public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequestDTO>
{
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
            .NotEmpty()
            .When(e => e.CategoryId != null);
        RuleFor(requestEvent => requestEvent.Date)
            .GreaterThan(DateTime.UtcNow.AddDays(1))
            .WithMessage("Event must be at least 1 day later")
            .When(e => e.Date != null);
    }
}
