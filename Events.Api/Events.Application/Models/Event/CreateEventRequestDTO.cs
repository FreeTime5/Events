using Microsoft.AspNetCore.Http;


namespace Events.Application.Models.Event;

public class CreateEventRequestDTO
{
    public string Title { get; set; } = string.Empty;

    public string Describtion { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Place { get; set; } = string.Empty;

    public int? CategoryId { get; set; }

    public int MaxMembers { get; set; }

    public IFormFile? Image { get; set; }
}
