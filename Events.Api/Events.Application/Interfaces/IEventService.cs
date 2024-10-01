using Events.Application.Models;
using Events.Application.Servicies.EventService.DTOs;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface IEventService
{
    public Task<Result> Create(CreateEventRequestDTO eventRequestDTO, User user);

    public Task<Result> DeleteEvent(Guid eventId, User user);

    public Task<GetEventsResponseDTO?> GetEventById(Guid id);

    public Task<GetEventsResponseDTO?> GetEventsByName(string name);

    public Task<Result> UpdateEvent(UpdateEventRequestDTO eventRequestDTO);

    public Task<IEnumerable<GetEventsResponseDTO>> GetFilteredEvents(string sortItem, string sortValue);

    public Task<IEnumerable<GetEventsResponseDTO>> GetAllEvents();

    public Task<IEnumerable<GetEventsResponseDTO>> GetEventsWithPagination(int page);
}
