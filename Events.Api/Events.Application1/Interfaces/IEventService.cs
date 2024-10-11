using Events.Application.Models;
using Events.Application.Services.EventService.DTOs;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface IEventService
{
    public Task Create(CreateEventRequestDTO eventRequestDTO, User user);

    public Task DeleteEvent(Guid eventId, User user);

    public Task<GetEventsResponseDTO?> GetEventById(Guid id);

    public Task<GetEventsResponseDTO?> GetEventsByName(string name);

    public Task UpdateEvent(UpdateEventRequestDTO eventRequestDTO);

    public Task<IEnumerable<GetEventsResponseDTO>> GetFilteredEvents(string sortItem, string sortValue);

    public Task<IEnumerable<GetEventsResponseDTO>> GetAllEvents();

    public Task<IEnumerable<GetEventsResponseDTO>> GetEventsWithPagination(int page);
}
