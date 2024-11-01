using Events.Domain.Entities;
using System.Security.Claims;

namespace Events.Application.Services.CategoryService;

public interface ICategoryService
{
    Task AddCategory(string name, ClaimsPrincipal claims);

    Task DeleteCategory(string name, ClaimsPrincipal claims);

    IQueryable<Category> GetAllCategories();

    Task<Category> GetCategoryById(string id);

    Task<Category> GetCategoryByName(string name);
}
