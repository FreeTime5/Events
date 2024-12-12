using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.GetUserUseCase.Implementations;

internal class GetUserUseCase : IGetUserUseCase
{
    private readonly UserManager<Member> userManager;

    public GetUserUseCase(UserManager<Member> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<Member?> Execute(string userName)
    {
        return await userManager.FindByNameAsync(userName);
    }
}
