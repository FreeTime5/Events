using Events.Domain.Entities;
using Events.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories;

internal class EventRepository : GenericRepository<Event>, IEventRepsository
{
    public EventRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Event?> FindById(string id)
    {
        return await dbContext.Events.FindAsync(id);
    }

    public IEnumerable<Event> GetEventsWithPagination(int page, int eventsOnPage)
    {
        return dbContext.Events.Skip((page - 1) * eventsOnPage).Take(page).AsNoTracking();
    }
}
