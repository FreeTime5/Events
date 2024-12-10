using Events.Domain.Entities;

namespace Events.Application.UseCases.CategoryUseCases.GetCategoryByNameUseCase;

public interface IGetCategoryByNameUseCase
{
    Task<Category> Execute(string name);
}
