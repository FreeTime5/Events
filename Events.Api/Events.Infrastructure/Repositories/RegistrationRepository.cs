using Events.Domain.Entities;
using Events.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories;

internal class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
{
    public RegistrationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Registration?> FindByEventAndMemberIds(string eventId, string memberId)
    {
        return await dbContext.Registrations.FirstOrDefaultAsync(r => r.EventId == eventId && r.MemberId == memberId);
    }
}
