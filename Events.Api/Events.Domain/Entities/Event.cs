namespace Events.Domain.Entities;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public Category? Category { get; set; }

    public int? CategoryId { get; set; }

    public int MaxMembers { get; set; }

    public List<Registration> Registrations { get; set; } = [];

    public string? ImageUrl { get; set; }

    public User? Creator { get; set; }

    public string? CreatorId { get; set; }
}
