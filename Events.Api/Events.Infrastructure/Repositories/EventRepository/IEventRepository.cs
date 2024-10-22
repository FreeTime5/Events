using Events.Infrastructure.Entities;

namespace Events.Infrastructure.Repositories.EventRepository;

public interface IEventRepository
{
    IQueryable<EventDb> GetAll();

    Task<EventDb?> GetById(string id);

    Task<EventDb?> GetByIdWithRegistrations(string id);

    Task<EventDb?> GetByTitle(string title);

    Task Add(EventDb ev);

    Task Update(EventDb ev);

    Task Delete(EventDb eventEntity);
}
