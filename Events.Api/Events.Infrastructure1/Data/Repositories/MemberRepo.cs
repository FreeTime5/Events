using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Domain.Entities;
using Events.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class MemberRepo : IMemberRepo
{
    private readonly ApplicationDbContext _dbContext;

    public MemberRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> AddToEvent(string memberId, Guid eventId)
    {
        var eventEntity = await _dbContext.Events.FindAsync(eventId);
        var member = await _dbContext.Users.FindAsync(memberId);

        List<Error> errors = [];
        if (eventEntity == null)
        {
            errors.Add(new Error("There are no event with that id", "", "Event"));
        }
        if (member == null)
        {
            errors.Add(new Error("There are no user with that id", "", "User"));
        }

        var check = await _dbContext.Registrations.Where(r => r.Event.Id == eventId && r.User.Id == memberId).FirstOrDefaultAsync();
        if (check != null)
        {
            errors.Add(new Error("This user is already a member of this event", "", "Registration"));
        }
        if (errors.Count > 0)
        {
            return Result.Failure(errors);
        }

        await _dbContext.Registrations.AddAsync(new Registration()
        {
            Id = Guid.NewGuid(),
            Event = eventEntity,
            User = member,
            RegistrationDate = DateTime.UtcNow
        });
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<User>> GetAllFromEvent(Guid eventId)
    {
        var eventEntity = await _dbContext.Events.FindAsync(eventId);

        if (eventEntity == null)
        {
            return [];
        }

        var membersOfEvent = await _dbContext.Registrations.Where(r => r.Event.Id == eventId)
            .Select(r => r.User)
            .ToListAsync();

        membersOfEvent.ForEach(u => u.Registrations = _dbContext.Registrations.Where(r => r.User.Id == u.Id).ToList());

        return membersOfEvent;
    }

    public async Task<User?> GetById(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        return user;
    }

    public async Task<Result> RemoveFromEvent(string memberId, Guid evId)
    {
        var registration = await _dbContext.Registrations.Where(r => r.Event.Id == evId && r.User.Id == memberId).FirstOrDefaultAsync();

        if (registration == null)
        {
            return Result.Failure([new Error("There are no such registration", "", "Registration")]);
        }

        _dbContext.Registrations.Remove(registration);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
