using Events.Application.Models.Account;
using Events.Domain.Entities;
using System.Security.Claims;

namespace Events.Application.Services.Account;

public interface IAccountService
{
    Task AddAdmin(string password);

    Task<LogInResoponseDTO> RegisterUserAndSignIn(RegisterRequestDTO requestDTO);

    Task<LogInResoponseDTO> LogIn(LogInRequestDTO requestDTO);

    Task<LogInResoponseDTO> LogOut(ClaimsPrincipal claims);

    Task<LogInResoponseDTO> IsLogIn(string accessToken, string userName);

    Task<Member?> GetUser(ClaimsPrincipal claims);

    Task<LogInResoponseDTO> RefreshToken(RefreshTokenRequestDTO requestDTO);

    Task DeleteUser(string userName);
}
