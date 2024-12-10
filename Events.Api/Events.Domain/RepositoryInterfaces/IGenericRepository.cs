using System.Linq.Expressions;

namespace Events.Domain.RepositoryInterfaces;

public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Add new entity to database
    /// </summary>
    /// <param name="item">Itme that will be added</param>
    void Add(T item);

    /// <summary>
    /// Delete entity from database
    /// </summary>
    /// <param name="item">Item that will be deleted</param>
    void Delete(T item);

    /// <summary>
    /// Update entity in database
    /// </summary>
    /// <param name="item">Item that will be updated</param>
    void Update(T item);

    /// <summary>
    /// Get all entity from database
    /// </summary>
    /// <returns>IQueryable entities</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Get all entity from database without tracking
    /// </summary>
    /// <returns>IQueryable entities</returns>
    IEnumerable<T> GetAllAsNoTracking();

    /// <summary>
    /// Finds entities with given predicate
    /// </summary>
    /// <param name="predicate">Predicate that will be used</param>
    /// <returns></returns>
    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Finds entities with given predicate without tracking
    /// </summary>
    /// <param name="predicate">Predicate that will be used</param>
    /// <returns></returns>
    IEnumerable<T> FindByAsNoTracking(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get first entity by predicate
    /// </summary>
    /// <param name="predicate">Predicate that will be used</param>
    /// <returns>T entity</returns>
    Task<T?> FirstOrDefault(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get first entity by predicate without tracking
    /// </summary>
    /// <param name="predicate">Predicate that will be used</param>
    /// <returns></returns>
    Task<T?> FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate);
}