using Events.Domain.RepositoryInterfaces;
using Events.Domain.UnitOfWorkInterface;
using Events.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Events.Infrastructure.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    private bool disposed = false;

    private readonly ApplicationDbContext dbContext;

    private ICategoryRepository? categoryRepository = null;

    private IEventRepsository? eventRepository = null;

    private IRegistrationRepository? registrationRepository = null;

    public ICategoryRepository CategoryRepository
    {
        get
        {
            return categoryRepository ?? throw new RepositoryNotImplementedException("CategoryRepository");
        }

        private set
        {
            if (value is null)
            {
                throw new RepositoryNotImplementedException("CategoryRepository");
            }

            categoryRepository = value;
        }
    }

    public IEventRepsository EventRepsository
    {
        get
        {
            return eventRepository ?? throw new RepositoryNotImplementedException("EventRepository");
        }

        private set
        {
            if (value is null)
            {
                throw new RepositoryNotImplementedException("EventRepository");
            }

            eventRepository = value;
        }
    }

    public IRegistrationRepository RegistrationRepository
    {
        get
        {
            return registrationRepository ?? throw new RepositoryNotImplementedException("RegistrationRepository");
        }

        private set
        {
            if (value is null)
            {
                throw new RepositoryNotImplementedException("RegistrationRepository");
            }

            registrationRepository = value;
        }
    }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void SetCategoryRepository()
    {
        CategoryRepository = dbContext.GetService<ICategoryRepository>();
    }

    public void SetEventRepository()
    {
        EventRepsository = dbContext.GetService<IEventRepsository>();
    }

    public void SetRegistrationRepository()
    {
        RegistrationRepository = dbContext.GetService<IRegistrationRepository>();
    }

    public async Task Save()
    {
        await dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }

        disposed = true;
    }
}
