using Events.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Events.Infrastructure.Repositories;

internal class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext dbContext;

    private readonly DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = dbContext.Set<T>();
    }

    public void Add(T item)
    {
        dbSet.Add(item);
    }

    public void Delete(T item)
    {
        dbSet.Remove(item);
    }

    public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Where(predicate);
    }

    public IEnumerable<T> FindByAsNoTracking(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Where(predicate).AsNoTracking();
    }

    public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public IEnumerable<T> GetAll()
    {
        return dbSet;
    }

    public IEnumerable<T> GetAllAsNoTracking()
    {
        return dbSet.AsNoTracking();
    }

    public void Update(T item)
    {
        dbSet.Update(item);
    }
}
