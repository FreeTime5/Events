using Events.Application.Models;
using Events.Application.Servicies.AccountService.DTOs;

namespace Events.Application.Interfaces;

public interface IAccountService
{
    Task<Result> RegisterUserAndSignIn(RegisterRequestDTO requestDTO);

    Task<Result> LogIn(LogInRequestDTO requestDTO);

    Task LogOut();
}
