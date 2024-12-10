using Events.Domain.Entities;
using Events.Domain.RepositoryInterfaces;

namespace Events.Infrastructure.Repositories;

internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Category?> FindById(string id)
    {
        return await dbContext.Categories.FindAsync(id);
    }
}
