using Events.Domain.Entities;

namespace Events.Infrastructure.Repositories.EventRepository;

public interface IEventRepository
{
    IQueryable<Event> GetAll();

    Task<Event?> GetById(string id);

    Task<Event?> GetByIdWithRegistrations(string id);

    Task<Event?> GetByName(string name);

    Task<bool> Add(Event ev);

    Task Update(Event ev);

    Task Delete(string eventId);
}
