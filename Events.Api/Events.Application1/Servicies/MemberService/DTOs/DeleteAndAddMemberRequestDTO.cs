namespace Events.Application.Services.MemberService.DTOs;

public class DeleteAndAddMemberRequestDTO
{
    public string MemberId { get; set; }

    public Guid EventId { get; set; }
}
