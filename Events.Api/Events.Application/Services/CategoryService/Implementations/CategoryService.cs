using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Events.Infrastructure.UnitOfWorkPattern;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.Services.CategoryService.Implementations;

internal class CategoryService : ICategoryService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<User> userManager;

    public CategoryService(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
    }


    public async Task AddCategory(string name, User user)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidDataException("ty ohuel?");
        }

        if (await userManager.IsInRoleAsync(user, "Admin"))
        {
            await unitOfWork.CategoryRepository.Add(name);
            return;
        }

        throw new UserHaveNoPermissionException();
    }

    public async Task DeleteCategory(string name, User user)
    {
        if (await userManager.IsInRoleAsync(user, "Admin"))
        {
            await unitOfWork.CategoryRepository.Delete(name);
            return;
        }

        throw new UserHaveNoPermissionException();
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        var categories = await unitOfWork.CategoryRepository.GetAll();

        return categories;
    }

    public async Task<Category> GetCategoryByName(string name)
    {
        Category? category = null;
        if (!string.IsNullOrEmpty(name))
        {
            category = await unitOfWork.CategoryRepository.GetByName(name);
        }

        if (category != null)
        {
            return category;
        }
        throw new ItemNotFoundException("Category");
    }

    public async Task<Category> GetCategoryById(string id)
    {
        var catgory = await unitOfWork.CategoryRepository.GetById(id);

        if (catgory != null)
        {
            return catgory;
        }

        throw new ItemNotFoundException("Category");
    }
}
