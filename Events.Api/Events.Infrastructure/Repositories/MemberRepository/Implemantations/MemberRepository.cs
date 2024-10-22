using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.MemberRepository.Implemantations;

internal class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext dbContext;

    public MemberRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddToEvent(string memberId, string eventId)
    {
        var eventEntity = await dbContext.Events.FindAsync(eventId);
        var member = await dbContext.Users.FindAsync(memberId);

        if (eventEntity == null)
        {
            throw new ItemNotFoundException("Event");
        }
        if (member == null)
        {
            throw new ItemNotFoundException("User");
        }

        var check = await dbContext.Registrations.Where(r => r.Event.Id == eventId && r.User.Id == memberId).FirstOrDefaultAsync();
        if (check != null)
        {
            throw new InvalidOperationException("This user is already a member of the event");
        }

        dbContext.Registrations.Add(new Registration()
        {
            Event = eventEntity,
            User = member,
            RegistrationDate = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();
    }

    public async Task<IQueryable<User>> GetAllFromEvent(string eventId)
    {
        var eventEntity = await dbContext.Events.FindAsync(eventId);

        if (eventEntity == null)
        {
            throw new ItemNotFoundException("Event");
        }

        var members = dbContext.Registrations.Where(r => r.Event.Id == eventId)
            .Include(r => r.User)
            .Select(r => r.User);

        return members;
    }

    public async Task<User?> GetById(string id)
    {
        var user = await dbContext.Users.FindAsync(id);

        return user;
    }

    public async Task Update(User user)
    {
        var userEntity = await dbContext.Users.FindAsync(user.Id);

        if (userEntity == null)
        {
            throw new ItemNotFoundException("User");
        }

        userEntity.FirstName = user.FirstName;
        userEntity.LastName = user.LastName;
        userEntity.Birthday = user.Birthday;

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveFromEvent(string memberId, string evId)
    {
        var registration = await dbContext.Registrations.Where(r => r.Event.Id == evId && r.User.Id == memberId).FirstOrDefaultAsync();

        if (registration == null)
        {
            throw new ItemNotFoundException("Registration");
        }

        dbContext.Registrations.Remove(registration);
        await dbContext.SaveChangesAsync();
    }
}
