using Events.Application.Models.Member;

namespace Events.Application.UseCases.MemberUseCases.GetMembersOfEventUseCase;

public interface IGetMembersOfEventUseCase
{
    Task<IEnumerable<GetMemberDTO>> Execute(string eventId);
}
