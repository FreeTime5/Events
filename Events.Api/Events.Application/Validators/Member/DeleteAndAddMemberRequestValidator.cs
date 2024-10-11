using Events.Application.Models.Member;
using FluentValidation;

namespace Events.Application.Validators.Member;

internal class DeleteAndAddMemberRequestValidator : AbstractValidator<DeleteAndAddMemberRequestDTO>
{
    private const string guidRegex = "^[a-f0-9]{8}-([a-f0-9]{4}-){3}[a-f0-9]{12}$";

    public DeleteAndAddMemberRequestValidator()
    {
        RuleFor(r => r.EventId).Matches(guidRegex);
        RuleFor(r => r.MemberId).Matches(guidRegex);
    }
}
