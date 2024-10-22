namespace Events.Domain.Entities;

public class Event
{
    public string Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public Category? Category { get; set; }

    public string? CategoryId { get; set; }

    public int MaxMembers { get; set; }

    public List<Registration> Registrations { get; set; } = [];

    public string ImageUrl { get; set; } = string.Empty;

    public User? Creator { get; set; }

    public string? CreatorId { get; set; }
}
