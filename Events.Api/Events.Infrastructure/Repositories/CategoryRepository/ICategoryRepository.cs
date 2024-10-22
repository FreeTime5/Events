using Events.Infrastructure.Entities;

namespace Events.Infrastructure.Repositories.CategoryRepository;

public interface ICategoryRepository
{
    public Task Add(CategoryDb category);

    public Task Delete(CategoryDb category);

    public IEnumerable<CategoryDb> GetAll();

    public Task<CategoryDb?> GetById(string id);

    public Task<CategoryDb?> GetByName(string name);
}
