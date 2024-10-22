using Events.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.EventRepository.Implementations;

internal class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext dbContext;

    public EventRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(EventDb ev)
    {
        dbContext.Events.Add(ev);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(EventDb eventEntity)
    {
        dbContext.Events.Remove(eventEntity);
        await dbContext.SaveChangesAsync();
    }

    public IQueryable<EventDb> GetAll()
    {
        return dbContext.Events;
    }

    public async Task<EventDb?> GetById(string id)
    {
        return await dbContext.Events.FindAsync(id);
    }

    public async Task<EventDb?> GetByIdWithRegistrations(string id)
    {
        return await dbContext.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EventDb?> GetByTitle(string title)
    {
        return await dbContext.Events.FirstOrDefaultAsync(e => e.Title == title);
    }

    public async Task Update(EventDb ev)
    {
        dbContext.Events.Update(ev);
        await dbContext.SaveChangesAsync();
    }
}
