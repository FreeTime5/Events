using Events.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.RegistrationRepository.Implementations;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly ApplicationDbContext dbContext;

    public RegistrationRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(RegistrationDb registration)
    {
        dbContext.Registrations.Add(registration);
        dbContext.Events.Update(registration.Event);
        await dbContext.SaveChangesAsync();
    }

    public async Task Remove(RegistrationDb registration)
    {
        dbContext.Registrations.Remove(registration);
        dbContext.Events.Update(registration.Event);
        await dbContext.SaveChangesAsync();
    }

    public async Task<RegistrationDb?> Find(string MemberId, string EventId)
    {
        return await dbContext.Registrations.FirstOrDefaultAsync(r => r.Event.Id == EventId && r.Member.Id == MemberId);
    }
}
