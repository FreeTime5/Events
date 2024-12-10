namespace Events.Application.Models.Member;

public class DeleteAndAddMemberRequestDTO
{
    public string MemberName { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;
}
