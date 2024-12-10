using Events.Application.Models.Account;

namespace Events.Application.UseCases.AccountUseCases.CheckIsLoginUseCase;

public interface ICheckIsLoginUseCase
{
    Task<LogInResoponseDTO> Execute(string accessToken, string userName);
}
