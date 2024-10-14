namespace Events.Application.Models.Member;

public class UpdateMemberDTO
{
    public string Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }
}
