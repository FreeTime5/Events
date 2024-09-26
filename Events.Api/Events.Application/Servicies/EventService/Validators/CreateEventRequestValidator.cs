using Events.Application.Servicies.EventService.DTOs;
using FluentValidation;

namespace Events.Application.Servicies.EventService.Validators;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequestDTO>
{
    public CreateEventRequestValidator()
    {
        RuleFor(requestEvent => requestEvent.Title).MaximumLength(100).NotEmpty();
        RuleFor(requestEvent => requestEvent.Describtion).MaximumLength(255);
        RuleFor(requestEvent => requestEvent.EventImage.Url).NotEmpty();
        RuleFor(requestEvent => requestEvent.MaxMembers).GreaterThan(5).WithMessage("Event must be at least with 5 members");
        RuleFor(requestEvent => requestEvent.Place).NotEmpty();
        RuleFor(requestEvent => requestEvent.Category.Name).NotEmpty();
        RuleFor(requestEvent => requestEvent.Date).GreaterThan(DateTime.Now.AddDays(1)).WithMessage("Event must be at least 1 day later");
    }
}
