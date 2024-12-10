using Events.Application.Exceptions;
using Events.Application.Models.Account;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.CheckIsLoginUseCase.Implementations;

internal class CheckIsLoginUseCase : ICheckIsLoginUseCase
{
    private readonly UserManager<Member> userManager;

    public CheckIsLoginUseCase(UserManager<Member> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<LogInResoponseDTO> Execute(string accessToken, string userName)
    {
        var loginResponse = new LogInResoponseDTO();

        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        loginResponse.RefreshToken = user.RefreshToken;
        loginResponse.JwtToken = accessToken;
        loginResponse.IsLogedIn = true;

        return loginResponse;
    }
}
