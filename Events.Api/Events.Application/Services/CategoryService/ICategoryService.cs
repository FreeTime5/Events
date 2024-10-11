using Events.Domain.Entities;

namespace Events.Application.Services.CategoryService;

public interface ICategoryService
{
    Task AddCategory(string name, User user);

    Task DeleteCategory(string name, User user);

    Task<IEnumerable<Category>> GetAllCategories();

    Task<Category> GetCategoryById(string id);

    Task<Category> GetCategoryByName(string name);
}
