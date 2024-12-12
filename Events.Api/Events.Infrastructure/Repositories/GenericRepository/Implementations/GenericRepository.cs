using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Events.Infrastructure.Repositories.GenericRepository.Implementations;

internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> entities;

    public GenericRepository(DbContext dbContext)
    {
        entities = dbContext.Set<TEntity>();
    }

    public void Add(TEntity item)
    {
        entities.Add(item);
    }

    public void Delete(TEntity item)
    {
        entities.Remove(item);
    }

    public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
    {
        return entities.Where(predicate);
    }

    public IQueryable<TEntity> FindByAsNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return entities.AsNoTracking().Where(predicate);
    }

    public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await entities.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> FirstOrDefaultAsNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return await entities.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public IQueryable<TEntity> GetAll()
    {
        return entities;
    }

    public IQueryable<TEntity> GetAllAsNoTracking()
    {
        return entities.AsNoTracking();
    }

    public void Update(TEntity item)
    {
        entities.Update(item);
    }
}
