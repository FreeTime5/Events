using Events.Application.Models;
using Events.Application.Services.AccountService.DTOs;
using Events.Domain.Entities;
using System.Security.Claims;

namespace Events.Application.Interfaces;

public interface IAccountService
{
    Task<Result> RegisterUserAndSignIn(RegisterRequestDTO requestDTO);

    Task<Result> LogIn(LogInRequestDTO requestDTO);

    Task LogOut();

    bool IsSignIn(ClaimsPrincipal claims);

    Task<User?> GetUser(ClaimsPrincipal claims);
}
