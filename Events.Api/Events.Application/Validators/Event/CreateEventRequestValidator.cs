using Events.Application.Models.Event;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Events.Application.Validators.Event
{
    internal class CreateEventRequestValidator : AbstractValidator<CreateEventRequestDTO>
    {
        private const string GUIDREGEX = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$";

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
                .GreaterThan(1)
                .WithMessage("Event must be at least with 2 members");
            RuleFor(requestEvent => requestEvent.Place)
                .NotEmpty();
            RuleFor(requestEvent => requestEvent.CategoryName)
                .NotEmpty()
                .When(requestEvent => requestEvent.CategoryName != null);
            RuleFor(requestEvent => requestEvent.Date)
                .GreaterThan(DateTime.UtcNow.AddDays(1))
                .WithMessage("Event must be at least 1 day later");
        }
    }
}
