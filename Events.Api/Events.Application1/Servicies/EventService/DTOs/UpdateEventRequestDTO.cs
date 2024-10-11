using Events.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Events.Application.Services.EventService.DTOs;

public class UpdateEventRequestDTO
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Describtion { get; set; }

    public DateTime? Date { get; set; }

    public string? Place { get; set; }

    public int? CategoryId { get; set; }

    public IFormFile? Image { get; set; }
}
