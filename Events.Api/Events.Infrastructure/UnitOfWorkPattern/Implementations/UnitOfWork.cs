using Events.Infrastructure.Repositories.CategoryRepository;
using Events.Infrastructure.Repositories.EventRepository;
using Events.Infrastructure.Repositories.MemberRepository;
using Events.Infrastructure.Repositories.RegistrationRepository;

namespace Events.Infrastructure.UnitOfWorkPattern.Implementations;

internal class UnitOfWork : IUnitOfWork, IDisposable
{
    private bool disposed = false;
    private readonly ApplicationDbContext dbContext;

    public IEventRepository EventRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IMemberRepository MemberRepository { get; }
    public IRegistrationRepository RegistrationRepository { get; }

    public UnitOfWork(IEventRepository eventRepository, ICategoryRepository categoryRepository, IMemberRepository memberRepository, IRegistrationRepository registrationRepository, ApplicationDbContext dbContext)
    {
        EventRepository = eventRepository;
        CategoryRepository = categoryRepository;
        MemberRepository = memberRepository;
        RegistrationRepository = registrationRepository;
        this.dbContext = dbContext;
    }

    public async Task SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
