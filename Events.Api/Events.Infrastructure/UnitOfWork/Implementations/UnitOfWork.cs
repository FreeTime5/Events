using Events.Infrastructure.Repositories.GenericRepository;
using Events.Infrastructure.Repositories.GenericRepository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.UnitOfWork.Implementations;

internal class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
{
    private bool disposed = false;
    private Dictionary<Type, object> repositories = [];

    public TContext DbContext { get; }

    public UnitOfWork(TContext dbContext)
    {
        DbContext = dbContext;
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);

        if (!repositories.ContainsKey(type))
        {
            repositories[type] = new GenericRepository<TEntity>(DbContext);
        }

        return (IGenericRepository<TEntity>)repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await DbContext.SaveChangesAsync();
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
                repositories?.Clear();

                DbContext.Dispose();
            }
        }

        disposed = true;
    }
}
