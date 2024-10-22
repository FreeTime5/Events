using Events.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories.CategoryRepository.Implementations;

internal class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(CategoryDb category)
    {
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(CategoryDb category)
    {
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
    }

    public IEnumerable<CategoryDb> GetAll()
    {
        return dbContext.Categories;
    }

    public async Task<CategoryDb?> GetById(string id)
    {
        return await dbContext.Categories.FindAsync(id);
    }

    public async Task<CategoryDb?> GetByName(string name)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }
}
