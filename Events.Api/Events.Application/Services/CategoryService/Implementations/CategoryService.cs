using Events.Application.Exceptions;
using Events.Infrastructure.Entities;
using Events.Infrastructure.UnitOfWorkPattern;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Services.CategoryService.Implementations;

internal class CategoryService : ICategoryService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<MemberDb> userManager;

    public CategoryService(IUnitOfWork unitOfWork, UserManager<MemberDb> userManager)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
    }

    public async Task AddCategory(string name, ClaimsPrincipal claims)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidDataException("Category name must not be null or empty");
        }

        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new UserHaveNoPermissionException();
        }

        var sameCategory = await unitOfWork.CategoryRepository.GetByName(name);

        if (sameCategory != null)
        {
            throw new ItemAlreadyAddedException("Category");
        }

        var category = new CategoryDb() { Name = name };

        await unitOfWork.CategoryRepository.Add(category);
    }

    public async Task DeleteCategory(string name, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new UserHaveNoPermissionException();
        }

        var category = await unitOfWork.CategoryRepository.GetByName(name) ?? throw new ItemNotFoundException("Category");

        await unitOfWork.CategoryRepository.Delete(category);
    }

    public IEnumerable<CategoryDb> GetAllCategories()
    {
        return unitOfWork.CategoryRepository.GetAll();
    }

    public async Task<CategoryDb> GetCategoryByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidDataException("Category name must not be null or empty");
        }

        return await unitOfWork.CategoryRepository.GetByName(name) ?? throw new ItemNotFoundException("Category");
    }

    public async Task<CategoryDb> GetCategoryById(string id)
    {
        return await unitOfWork.CategoryRepository.GetById(id) ?? throw new ItemNotFoundException("Category");
    }
}
