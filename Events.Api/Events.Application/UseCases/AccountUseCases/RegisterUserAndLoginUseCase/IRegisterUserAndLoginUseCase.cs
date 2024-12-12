using Events.Application.Models.Account;

namespace Events.Application.UseCases.AccountUseCases.RegisterUserAndLoginUseCase;

public interface IRegisterUserAndLoginUseCase
{
    Task<LogInResoponseDTO> Execute(RegisterRequestDTO requestDTO);
}
