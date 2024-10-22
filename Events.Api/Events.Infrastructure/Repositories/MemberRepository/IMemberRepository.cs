using Events.Infrastructure.Entities;

namespace Events.Infrastructure.Repositories.MemberRepository;

public interface IMemberRepository
{
    IQueryable<MemberDb> GetAllFromEvent(EventDb eventEntity);

    Task AddToEvent(MemberDb member, EventDb eventEntity);

    Task<MemberDb?> GetById(string id);

    Task<MemberDb?> GetByName(string userName);

    Task Update(MemberDb member);
}
