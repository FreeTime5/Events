using FluentValidation;

namespace Events.Application.Validators.Event
{
    internal class PageValidator : AbstractValidator<int>
    {
        public PageValidator()
        {
            RuleFor(page => page >= 1);
        }
    }
}
