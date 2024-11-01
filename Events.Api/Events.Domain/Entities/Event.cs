namespace Events.Domain.Entities;

public class Event
{
    public string Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public int MaxMembers { get; set; }

    public virtual Category? Category { get; set; }

    public string? CategoryId { get; set; }

    public Member? Creator { get; set; }

    public string? CreatorId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string ImageName { get; set; } = string.Empty;

    public virtual List<Registration> Registrations { get; set; } = [];

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
}