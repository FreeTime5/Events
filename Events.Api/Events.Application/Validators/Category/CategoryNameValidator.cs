using FluentValidation;

namespace Events.Application.Validators.Category;

internal class CategoryNameValidator : AbstractValidator<string>
{
    public CategoryNameValidator()
    {
        RuleFor(name => !string.IsNullOrEmpty(name));
    }
}
