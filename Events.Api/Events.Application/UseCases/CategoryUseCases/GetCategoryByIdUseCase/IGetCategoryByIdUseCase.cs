using Events.Domain.Entities;

namespace Events.Application.UseCases.CategoryUseCases.GetCategoryByIdUseCase;

public interface IGetCategoryByIdUseCase
{
    Task<Category> Execute(string id);
}
