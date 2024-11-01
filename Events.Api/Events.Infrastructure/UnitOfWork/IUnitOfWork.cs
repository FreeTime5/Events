using Events.Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.UnitOfWork;

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    public TContext DbContext { get; }
}

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync();
}
