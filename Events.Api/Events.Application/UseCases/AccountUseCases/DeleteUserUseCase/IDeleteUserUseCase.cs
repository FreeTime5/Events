
namespace Events.Application.UseCases.AccountUseCases.DeleteUserUseCase;

public interface IDeleteUserUseCase
{
    Task Execute(string userName);
}
