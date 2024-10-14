using Events.Domain.Entities;

namespace Events.Infrastructure.Repositories.MemberRepository;

public interface IMemberRepository
{
    Task<IQueryable<User>> GetAllFromEvent(string eventId);

    Task AddToEvent(string memberId, string eventId);

    Task RemoveFromEvent(string memberId, string evId);

    Task<User?> GetById(string id);

    Task Update(User user);
}
