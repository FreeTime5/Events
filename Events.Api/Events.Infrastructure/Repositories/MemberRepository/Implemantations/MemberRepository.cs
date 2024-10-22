using Events.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.MemberRepository.Implemantations;

internal class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext dbContext;

    public MemberRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddToEvent(MemberDb member, EventDb eventEntity)
    {
        dbContext.Registrations.Add(new RegistrationDb()
        {
            Event = eventEntity,
            Member = member,
            RegistrationDate = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();
    }

    public IQueryable<MemberDb> GetAllFromEvent(EventDb eventEntity)
    {
        var members = dbContext.Registrations.Where(r => r.Event.Id == eventEntity.Id)
            .Select(r => r.Member);

        return members;
    }

    public async Task<MemberDb?> GetById(string id)
    {
        return await dbContext.Users.FindAsync(id);
    }

    public async Task Update(MemberDb member)
    {
        dbContext.Update(member);
        await dbContext.SaveChangesAsync();
    }

    public async Task<MemberDb?> GetByName(string userName)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }
}
