using Events.Domain.Entities;

namespace Events.Application.UseCases.CategoryUseCases.GetAllCategoriesUseCase;

public interface IGetAllCategoriesUseCase
{
    IEnumerable<Category> Execute();
}
