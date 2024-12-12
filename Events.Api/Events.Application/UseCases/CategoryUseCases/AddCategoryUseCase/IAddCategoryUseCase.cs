namespace Events.Application.UseCases.CategoryUseCases.AddCategoryUseCase;

public interface IAddCategoryUseCase
{
    Task Execute(string name, string userName);
}
