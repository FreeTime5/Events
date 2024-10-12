using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.EventRepository.Implementations;

internal class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext dbContext;

    public EventRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> Add(Event ev)
    {
        var eventEntity = await dbContext.Events.Where(e => e.Title == ev.Title).FirstOrDefaultAsync();

        if (eventEntity != null)
        {
            throw new ItemAlreadyAddedException("Event");
        }

        dbContext.Events.Add(ev);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task Delete(string eventId)
    {
        var eventEntity = await dbContext.Events.FindAsync(eventId);

        if (eventEntity == null)
        {
            throw new ItemNotFoundException("Event");
        }

        dbContext.Events.Remove(eventEntity);
        await dbContext.SaveChangesAsync();
    }

    public IQueryable<Event> GetAll()
    {
        return dbContext.Events;
    }

    public async Task<Event?> GetById(string id)
    {
        return await dbContext.Events.FindAsync(id);
    }

    public async Task<Event?> GetByIdWithRegistrations(string id)
    {
        var eventInstance = await dbContext.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
        return eventInstance;

    }

    public async Task<Event?> GetByName(string name)
    {
        var eventEntity = await dbContext.Events.Where(e => e.Title == name).FirstOrDefaultAsync();

        return eventEntity;
    }

    public async Task Update(Event ev)
    {
        var eventEntity = await dbContext.Events.FindAsync(ev.Id);

        if (eventEntity == null)
        {
            throw new ItemNotFoundException("Event");
        }

        eventEntity.Title = ev.Title;
        eventEntity.Describtion = ev.Describtion;
        eventEntity.ImageUrl = ev.ImageUrl;
        eventEntity.CategoryId = ev.CategoryId;
        eventEntity.Date = ev.Date;
        eventEntity.Place = ev.Place;

        await dbContext.SaveChangesAsync();
    }
}
