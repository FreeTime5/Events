using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Services.ServiciesErrors;
using Events.Domain.Entities;
using Events.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

internal class EventRepo : IEventRepo
{
    private readonly ApplicationDbContext _dbContext;

    public EventRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Event ev)
    {
        var eventEntity = await _dbContext.Events.Where(e => e.Title == ev.Title).FirstOrDefaultAsync();

        if (eventEntity != null)
        {
            throw new EntityAlreadyAddedException("Event");
        }

        await _dbContext.Events.AddAsync(ev);
        _dbContext.SaveChangesAsync();
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

    public IQueryable<Event> GetAll()
    {
        return _dbContext.Events.Include(ev => ev.Registrations);
    }

    public async Task<Event?> GetById(Guid id)
    {
        return await _dbContext.Events.Include(ev => ev.Registrations).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Event?> GetByName(string name)
    {
        var eventEntity = await _dbContext.Events.Where(e => e.Title == name)
            .Include(ev => ev.Registrations)
            .FirstOrDefaultAsync();

        return eventEntity;
    }

    public async Task Update(Event ev)
    {
        var eventEntity = await _dbContext.Events.FindAsync(ev.Id);

        if (eventEntity == null)
        {
            throw new Exception();
        }

        eventEntity.Title = ev.Title;
        eventEntity.Describtion = ev.Describtion;
        eventEntity.ImageUrl = ev.ImageUrl;
        eventEntity.CategoryId = ev.CategoryId;
        eventEntity.Date = ev.Date;
        eventEntity.Place = ev.Place;

        await _dbContext.SaveChangesAsync();
    }
}
