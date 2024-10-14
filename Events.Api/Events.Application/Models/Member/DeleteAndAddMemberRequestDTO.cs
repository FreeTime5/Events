namespace Events.Application.Models.Member;

public class DeleteAndAddMemberRequestDTO
{
    public string MemberId { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;
}
