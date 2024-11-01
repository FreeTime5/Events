using Events.Application.Exceptions;
using Events.Domain.Entities;
using Events.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Services.CategoryService.Implementations;

internal class CategoryService : Service, ICategoryService
{
    private readonly UserManager<Member> userManager;

    public CategoryService(IUnitOfWork unitOfWork, UserManager<Member> userManager)
        :base(unitOfWork)
    {
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

        var sameCategory = await unitOfWork.GetRepository<Category>()
            .FirstOrDefaultAsNoTracking(c => c.Name == name);

        if (sameCategory != null)
        {
            throw new ItemAlreadyAddedException("Category");
        }

        var category = new Category() { Name = name };

        unitOfWork.GetRepository<Category>().Add(category);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCategory(string name, ClaimsPrincipal claims)
    {
        var user = await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new UserHaveNoPermissionException();
        }

        var category = await unitOfWork.GetRepository<Category>()
            .FirstOrDefault(c => c.Name == name) 
            ?? throw new ItemNotFoundException("Category");

        unitOfWork.GetRepository<Category>().Delete(category);
        await unitOfWork.SaveChangesAsync();
    }

    public IQueryable<Category> GetAllCategories()
    {
        return unitOfWork.GetRepository<Category>().GetAllAsNoTracking();
    }

    public async Task<Category> GetCategoryByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidDataException("Category name must not be null or empty");
        }

        return await unitOfWork.GetRepository<Category>()
            .FirstOrDefaultAsNoTracking(c => c.Name == name)
            ?? throw new ItemNotFoundException("Category");
    }

    public async Task<Category> GetCategoryById(string id)
    {
        return await unitOfWork.GetRepository<Category>()
            .FirstOrDefaultAsNoTracking(c => c.Id == id) 
            ?? throw new ItemNotFoundException("Category");
    }
}
