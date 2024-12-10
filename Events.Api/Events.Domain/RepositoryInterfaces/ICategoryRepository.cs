using Events.Domain.Entities;

namespace Events.Domain.RepositoryInterfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> FindById(string id);
}
