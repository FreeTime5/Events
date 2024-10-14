using Events.Application.Models.Member;

namespace Events.Application.Services.MemberService;

public interface IMemberService
{

    Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(string eventId);

    Task DeleteMemberFromEvent(DeleteAndAddMemberRequestDTO requestDTO);

    Task AddMemberToEvent(DeleteAndAddMemberRequestDTO requestDTO);

    Task UpdateMemberInformation(UpdateMemberDTO requestDTO);
}
