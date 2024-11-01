namespace Events.Application.Models.Member;

public class UpdateMemberDTO
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }
}
