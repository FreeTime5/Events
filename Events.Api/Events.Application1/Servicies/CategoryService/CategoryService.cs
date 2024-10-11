using Events.Application.Interfaces;
using Events.Application.Interfaces.RepoInterfaces;
using Events.Application.Models;
using Events.Application.Services.ServiciesErrors;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly UserManager<User> _userManager;

    public CategoryService(ICategoryRepo categoryRepo, UserManager<User> userManager)
    {
        _categoryRepo = categoryRepo;
        _userManager = userManager;
    }

    
    public async Task AddCategory(string name, User user)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidDataException("ty ohuel?");
        }

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _categoryRepo.Add(name);
        }

        throw new InvalidDataException("ty kto takoi?");
    }

    public async Task<Result> DeleteCategory(string name, User user)
    {
        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            return await _categoryRepo.Delete(name);
        }

        return Result.Failure([AccountErrors.UserHaveNotPermision]);
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        var categories = await _categoryRepo.GetAll();

        return categories;
    }

    public async Task<Category?> GetCategoryByName(string name)
    {
        if (!string.IsNullOrEmpty(name)) 
            return await _categoryRepo.GetByName(name);

        return null;
    }
    
    public async Task<Category?> GetCategoryById(int id)
    {
        return await _categoryRepo.GetById(id);
    }
}
