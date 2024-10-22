using Events.Application.Models.Member;
using System.Security.Claims;

namespace Events.Application.Services.MemberService;

public interface IMemberService
{

    Task<IEnumerable<GetMemberDTO>> GetMembersOfEvent(string eventId);

    Task DeleteMemberFromEvent(string eventId, ClaimsPrincipal claims);

    Task AddMemberToEvent(string eventId, ClaimsPrincipal claims);

    Task UpdateMemberInformation(UpdateMemberDTO requestDTO, string userName);
}
