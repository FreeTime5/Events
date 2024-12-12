using Events.Domain.Entities;

namespace Events.Domain.RepositoryInterfaces;

public interface IEventRepsository : IGenericRepository<Event>
{
    Task<Event?> FindById(string id);

    IEnumerable<Event> GetEventsWithPagination(int page, int eventsOnPage);
}
