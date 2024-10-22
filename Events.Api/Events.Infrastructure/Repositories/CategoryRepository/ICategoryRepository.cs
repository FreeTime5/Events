using Events.Domain.Entities;

namespace Events.Infrastructure.Repositories.CategoryRepository;

public interface ICategoryRepository
{
    public Task Add(string name);

    public Task Delete(string name);

    public Task<IEnumerable<Category>> GetAll();

    public Task<Category?> GetById(string id);

    public Task<Category?> GetByName(string name);
}
