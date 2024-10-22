using Events.Application.Models.Member;
using FluentValidation;

namespace Events.Application.Validators.Member;

internal class DeleteAndAddMemberRequestValidator : AbstractValidator<DeleteAndAddMemberRequestDTO>
{
    private const string guidRegex = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$7";

    public DeleteAndAddMemberRequestValidator()
    {
        RuleFor(r => r.EventId).Matches(guidRegex);
        RuleFor(r => r.MemberId).Matches(guidRegex);
    }
}
