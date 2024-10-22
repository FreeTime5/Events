using Events.Domain.Entities;

namespace Events.Infrastructure.Entities;

public class MemberDb : Member
{
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }

    public List<RegistrationDb> Registrations { get; set; } = [];
}
