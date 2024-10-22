using Events.Infrastructure.Entities;

namespace Events.Infrastructure.Repositories.CategoryRepository;

public interface ICategoryRepository
{
    public Task Add(string name);

    public Task Delete(CategoryDb category);

    public Task<IEnumerable<CategoryDb>> GetAll();

    public Task<CategoryDb?> GetById(string id);

    public Task<CategoryDb?> GetByName(string name);
}
