using Events.Application.Models.Account;

namespace Events.Application.UseCases.AccountUseCases.LogoutUseCase;

public interface ILogoutUseCase
{
    Task<LogInResoponseDTO> Execute(string userName);
}
