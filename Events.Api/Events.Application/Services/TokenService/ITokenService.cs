using Events.Application.Models.Account;
using Events.Domain.Entities;

namespace Events.Application.Services.TokenService;

public interface ITokenService
{
    string GenerateJWT(string userName, string role);

    string GenerateRefreshToken();

    Task<LogInResoponseDTO> GenerateTokens(Member user);

    Task<LogInResoponseDTO> DeleteTokens(Member user);
}
