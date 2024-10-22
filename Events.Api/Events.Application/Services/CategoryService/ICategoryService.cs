using Events.Infrastructure.Entities;
using System.Security.Claims;

namespace Events.Application.Services.CategoryService;

public interface ICategoryService
{
    Task AddCategory(string name, ClaimsPrincipal claims);

    Task DeleteCategory(string name, ClaimsPrincipal claims);

    IEnumerable<CategoryDb> GetAllCategories();

    Task<CategoryDb> GetCategoryById(string id);

    Task<CategoryDb> GetCategoryByName(string name);
}
