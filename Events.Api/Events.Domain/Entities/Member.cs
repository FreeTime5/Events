using Microsoft.AspNetCore.Identity;


namespace Events.Domain.Entities;

public class Member : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }

    public List<Registration> Registrations { get; set; } = [];
}
