using Events.Application.Models.Account;
using Events.Domain.Entities;
using System.Security.Claims;

namespace Events.Application.Services.Account;

public interface IAccountService
{
    Task<LogInResoponseDTO> RegisterUserAndSignIn(RegisterRequestDTO requestDTO);

    Task<LogInResoponseDTO> LogIn(LogInRequestDTO requestDTO);

    Task<LogInResoponseDTO> LogOut(ClaimsPrincipal claims);

    Task<User?> GetUser(ClaimsPrincipal claims);

    Task<LogInResoponseDTO> RefreshToken(RefreshTokenRequestDTO requestDTO);

    Task DeleteUser(string userName);
}
