namespace Events.Domain.Entities;

public class Event
{
    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public int MaxMembers { get; set; }
}