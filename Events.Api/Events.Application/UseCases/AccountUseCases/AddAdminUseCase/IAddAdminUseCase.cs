namespace Events.Application.UseCases.AccountUseCases.AddAdminUseCase;

public interface IAddAdminUseCase
{
    Task Execute(string password);
}
