using Events.Application.Models.Event;
using Events.Domain.Entities;
using System.Security.Claims;

namespace Events.Application.Services.EventService;

public interface IEventService
{
    Task Create(CreateEventRequestDTO eventRequestDTO, User user);

    Task DeleteEvent(string eventId, User user);

    Task<GetEventsResponseDTO> GetEventById(string id);

    Task<GetEventsResponseDTO> GetEventsByName(string name);

    Task UpdateEvent(UpdateEventRequestDTO eventRequestDTO, ClaimsPrincipal claims);

    IEnumerable<GetEventsResponseDTO> GetFilteredEvents(int page, string sortItem, string sortValue);

    IEnumerable<GetEventsResponseDTO> GetAllEvents();

    IEnumerable<GetEventsResponseDTO> GetEventsWithPagination(int page);
}
