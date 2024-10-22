using Events.Infrastructure.Repositories.CategoryRepository;
using Events.Infrastructure.Repositories.EventRepository;
using Events.Infrastructure.Repositories.MemberRepository;
using Events.Infrastructure.Repositories.RegistrationRepository;

namespace Events.Infrastructure.UnitOfWorkPattern;

public interface IUnitOfWork
{
    IEventRepository EventRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IMemberRepository MemberRepository { get; }

    IRegistrationRepository RegistrationRepository { get; }

    Task SaveAsync();
}
