using Microsoft.AspNetCore.Http;

namespace Events.Application.Models.Event;

public class UpdateEventRequestDTO
{
    public string Id { get; set; }

    public string? Title { get; set; }

    public string? Describtion { get; set; }

    public DateTime? Date { get; set; }

    public string? Place { get; set; }

    public string? CategoryName { get; set; }

    public IFormFile? Image { get; set; }
}
