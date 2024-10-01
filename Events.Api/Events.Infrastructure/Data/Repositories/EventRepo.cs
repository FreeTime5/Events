using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.ServiciesErrors;
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
        var eventEntity = await _dbContext.Events.Where(e => e.Title == ev.Title).FirstOrDefaultAsync();

        if (eventEntity != null)
        {
            return Result.Failure([EventErrors.DuplicatedEventTitle]);
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
            return Result.Failure([EventErrors.EventNotFound]);
        }

        _dbContext.Events.Remove(eventEntity);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<List<Event>> GetAll()
    {
        
        var events = await _dbContext.Events.Include(ev => ev.Registrations)
            .ToListAsync();
        return events;
    }

    public async Task<Event?> GetById(Guid id)
    {
        var eventEntity = await _dbContext.Events.Where(e => e.Id == id)
            .Include(ev => ev.Registrations)
            .FirstOrDefaultAsync();
        return eventEntity;
    }

    public async Task<Event?> GetByName(string name)
    {
        var eventEntity = await _dbContext.Events.Where(e => e.Title == name)
            .Include(ev => ev.Registrations)
            .FirstOrDefaultAsync();

        return eventEntity;
    }

    public async Task<Result> Update(Event ev)
    {
        var eventEntity = await _dbContext.Events.FindAsync(ev.Id);

        if (eventEntity == null)
        {
            return Result.Failure([EventErrors.EventNotFound]);
        }

        var dublicatedEvent = await _dbContext.Events.Where(e => e.Title == ev.Title && e.Id != ev.Id).FirstOrDefaultAsync();

        if (dublicatedEvent != null)
        {
            return Result.Failure([EventErrors.DuplicatedEventTitle]);
        }

        eventEntity.Title = ev.Title;
        eventEntity.Describtion = ev.Describtion;
        eventEntity.ImageUrl = ev.ImageUrl;
        eventEntity.CategoryId = ev.CategoryId;
        eventEntity.Date = ev.Date;
        eventEntity.Place = ev.Place;

        var updatedRows = await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
