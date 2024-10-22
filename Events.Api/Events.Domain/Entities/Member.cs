using Microsoft.AspNetCore.Identity;


namespace Events.Domain.Entities;

public class Member : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }
}
