using Events.Application.Interfaces.RepoInterfaces;
using Events.Application.Models;
using Events.Domain.Entities;
using Events.Infrastructure.DatabaseErrors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.Data.Repositories;

public class CategoryRepo : ICategoryRepo
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepo(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(string name)
    {
        if (await GetByName(name) != null)
        {
            throw new EntityAlreadyAddedException("Category");
        }

        var category = new Category() { Name = name };
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Result> Delete(string name)
    {
        var category = await GetByName(name);

        if (category == null)
            return Result.Failure([CategoryErrors.CategoryNotFound]);

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetById(int id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }

    public async Task<Category?> GetByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var category = await _dbContext.Categories.Where(c => c.Name == name)
            .FirstOrDefaultAsync();

        return category;
    }
}
