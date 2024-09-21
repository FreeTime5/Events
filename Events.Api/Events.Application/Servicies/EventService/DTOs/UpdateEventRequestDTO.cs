using Events.Domain.Entities;

namespace Events.Application.Servicies.EventService.DTOs;

public class UpdateEventRequestDTO 
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public Category Category { get; set; }

    public int MaxMembers { get; set; }

    public Image EventImage { get; set; }
}
