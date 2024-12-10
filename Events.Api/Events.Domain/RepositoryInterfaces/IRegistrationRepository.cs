using Events.Domain.Entities;

namespace Events.Domain.RepositoryInterfaces;

public interface IRegistrationRepository : IGenericRepository<Registration>
{
    Task<Registration?> FindByEventAndMemberIds(string eventId, string memberId);
}
