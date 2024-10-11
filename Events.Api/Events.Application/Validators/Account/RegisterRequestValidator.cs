using Events.Application.Models.Account;
using FluentValidation;

namespace Events.Application.Validators.Account;

internal class RegisterRequestValidator : AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.UserName).NotEmpty();
        RuleFor(r => r.Password).NotEmpty();
        RuleFor(r => r.Email).NotEmpty();
    }
}
