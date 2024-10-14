using Events.Application.Models.Account;
using FluentValidation;

namespace Events.Application.Validators.Account;

internal class LogInRequestValidator : AbstractValidator<LogInRequestDTO>
{
    public LogInRequestValidator()
    {
        RuleFor(r => r.UserName).NotEmpty();
        RuleFor(r => r.Password).NotEmpty();
    }
}
