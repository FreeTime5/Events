namespace Events.Application.Models.Event;

public class GetEventsResponseDTO
{
    public string Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public string? CategoryName { get; set; } = string.Empty;

    public int MaxMembers { get; set; }

    public int RegistratinCount { get; set; }

    public string? EventImageUrl { get; set; } = null;

    public string? CreatorName { get; set; }
}
