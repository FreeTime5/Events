using Events.Application.Models.Account;

namespace Events.Application.UseCases.AccountUseCases.LoginUseCase;

public interface ILoginUseCase
{
    Task<LogInResoponseDTO> Execute(LogInRequestDTO requestDTO);
}
