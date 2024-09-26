namespace Events.Application.Servicies.MemberService.DTOs;

public class DeleteAndAddMemberRequestDTO
{
    public string MemberId { get; set; }

    public Guid EventId { get; set; }
}
