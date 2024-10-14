using Events.Infrastructure.Repositories.CategoryRepository;
using Events.Infrastructure.Repositories.EventRepository;
using Events.Infrastructure.Repositories.MemberRepository;

namespace Events.Infrastructure.UnitOfWorkPattern;

public interface IUnitOfWork
{
    IEventRepository EventRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IMemberRepository MemberRepository { get; }

    Task SaveAsync();
}
