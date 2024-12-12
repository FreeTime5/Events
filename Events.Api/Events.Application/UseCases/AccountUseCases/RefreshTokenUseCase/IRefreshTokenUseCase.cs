using Events.Application.Models.Account;

namespace Events.Application.UseCases.AccountUseCases.RefreshTokenUseCase;

public interface IRefreshTokenUseCase
{
    Task<LogInResoponseDTO> Execute(RefreshTokenRequestDTO requestDTO);
}
