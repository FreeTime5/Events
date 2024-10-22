using Events.Domain.Entities;

namespace Events.Infrastructure.Entities;

public class EventDb : Event
{
    public string Id { get; set; }

    public CategoryDb? Category { get; set; }

    public string? CategoryId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string ImageName { get; set; } = string.Empty;

    public List<RegistrationDb> Registrations { get; set; } = [];

    public int RegistrationsCount { get; set; }

    public bool AddRegistration()
    {
        if (RegistrationsCount < MaxMembers)
        {
            RegistrationsCount++;
            return true;
        }
        return false;
    }

    public void RemoveRegistation()
    {
        RegistrationsCount--;
    }

    public MemberDb? Creator { get; set; }

    public string? CreatorId { get; set; }
}
