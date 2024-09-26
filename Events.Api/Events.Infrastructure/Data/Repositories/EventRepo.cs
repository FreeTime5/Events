using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Domain.Entities;
using Events.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventRepo : IEventRepo
{
    private readonly ApplicationDbContext _dbContext;

    public EventRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Add(Event ev)
    {
        var eventEntity = await _dbContext.Events.Where(e => e.Title == ev.Title).FirstAsync();

        if (eventEntity != null)
        {
            return Result.Failure([new Error("There are event with such title", "", "Event")]);
        }

        await _dbContext.Events.AddAsync(ev);
        var saveResult = await _dbContext.SaveChangesAsync();

        if (saveResult == 0)
        {
            return Result.Failure([new Error("Database add operation error", "", "Event")]);
        }

        return Result.Success();
    }

    public async Task<Result> Delete(Guid eventId)
    {
        var eventEntity = await _dbContext.Events.FindAsync(eventId);

        if (eventEntity == null)
        {
            return Result.Failure([new Error("There are no event with that id", "", "Event")]);
        }

        _dbContext.Events.Remove(eventEntity);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<List<Event>> GetAll()
    {
        var events = await _dbContext.Events.ToListAsync();
        return events;
    }

    public async Task<Event?> GetById(Guid id)
    {
        var eventEntity = await _dbContext.Events.FindAsync(id);

        return eventEntity;
    }

    public async Task<Event?> GetByName(string name)
    {
        var eventEntity = await _dbContext.Events.Where(e => e.Title == name).FirstAsync();

        return eventEntity;
    }

    public async Task<Result> Update(Event ev)
    {
        var eventEntity = _dbContext.Events.Where(e => e.Id == ev.Id);

        if (eventEntity == null)
        {
            return Result.Failure([new Error("There are no event with that id", "", "Event")]);
        }

        var updatedRows = await eventEntity.ExecuteUpdateAsync(updates =>
             updates.SetProperty(e => e.Title, ev.Title)
                    .SetProperty(e => e.Describtion, ev.Describtion)
                    .SetProperty(e => e.EventImage, ev.EventImage)
                    .SetProperty(e => e.MaxMembers, ev.MaxMembers)
                    .SetProperty(e => e.Category, ev.Category)
                    .SetProperty(e => e.Date, ev.Date)
                    .SetProperty(e => e.Place, ev.Place)
        );

        if (updatedRows == 0)
        {
            return Result.Failure([new Error("Database error", "", "Event")]);
        }
        return Result.Success();
    }
}
