using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;


namespace Events.Domain.Entities;

public class User : IdentityUser
{

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly Birthday { get; set; }

    public List<Registration> Registrations { get; set; } = [];
}
