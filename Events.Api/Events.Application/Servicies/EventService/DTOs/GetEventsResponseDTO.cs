using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Servicies.EventService.DTOs;

public class GetEventsResponseDTO
{
    public Guid Id { get; set; } 

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
