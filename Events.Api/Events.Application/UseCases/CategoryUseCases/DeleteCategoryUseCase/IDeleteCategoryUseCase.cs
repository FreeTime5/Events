namespace Events.Application.UseCases.CategoryUseCases.DeleteCategoryUseCase;

public interface IDeleteCategoryUseCase
{
    Task Execute(string name, string userName);
}
