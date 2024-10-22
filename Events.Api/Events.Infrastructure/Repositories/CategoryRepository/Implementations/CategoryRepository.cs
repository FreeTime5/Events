using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.CategoryRepository.Implementations;

internal class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(string name)
    {
        if (await GetByName(name) != null)
        {
            throw new ItemAlreadyAddedException("Category");
        }

        var category = new Category() { Name = name };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(string name)
    {
        var category = await GetByName(name);

        if (category == null)
            throw new ItemNotFoundException("Category");

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetById(string id)
    {
        return await dbContext.Categories.FindAsync(id);
    }

    public async Task<Category?> GetByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var category = await dbContext.Categories.Where(c => c.Name == name)
            .FirstOrDefaultAsync();

        return category;
    }
}
