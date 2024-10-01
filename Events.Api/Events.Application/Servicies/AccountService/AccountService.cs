using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.AccountService.DTOs;
using Events.Application.Servicies.ServiciesErrors;
using Events.Domain.Entities;
using Events.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Events.Application.Servicies.AccountService;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
    {

        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User?> GetUser(ClaimsPrincipal claims)
    {
        return await _userManager.GetUserAsync(claims);
    }

    public bool IsSignIn(ClaimsPrincipal claims)
    {
        return _signInManager.IsSignedIn(claims);
    }

    public async Task<Result> LogIn(LogInRequestDTO requestDTO)
    {
        var user = await _userManager.FindByNameAsync(requestDTO.UserName);

        if (user == null)
        {
            return Result.Failure([AccountErrors.UserNotFound]);
        }

        if (!await _userManager.CheckPasswordAsync(user, requestDTO.Password))
        {
            return Result.Failure([AccountErrors.IncorrectPassword]);
        }

        await _signInManager.SignInAsync(user, true);
        return Result.Success();
    }


    public async Task LogOut() => await _signInManager.SignOutAsync();

    public async Task<Result> RegisterUserAndSignIn(RegisterRequestDTO requestDTO)
    {
        var user = new User()
        {
            Email = requestDTO.Email,
            UserName = requestDTO.UserName,
        };
        var result = await _userManager.CreateAsync(user, requestDTO.Password);
        

        if (!result.Succeeded)
        {
            List<Error> errors = [];
            foreach (var error in result.Errors)
            {
                errors.Add(new Error(error.Description, error.Code, "User"));
            }
            return Result.Failure(errors);
        }
        await _userManager.AddToRoleAsync(user, "User");
        await _signInManager.SignInAsync(user, true);

        return Result.Success();
    }
}
