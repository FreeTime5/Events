using Events.Application.Exceptions;
using Events.Application.Models.Account;
using Events.Application.Services.TokenService;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.LogoutUseCase.Implementations;

internal class LogoutUseCase : ILogoutUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly ITokenService tokenService;

    public LogoutUseCase(UserManager<Member> userManager,
        ITokenService tokenService)
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
    }

    public async Task<LogInResoponseDTO> Execute(string userName)
    {
        var user = await userManager.FindByNameAsync(userName) ?? throw new UserNotSignedInException();

        return await tokenService.DeleteTokens(user);
    }
}
