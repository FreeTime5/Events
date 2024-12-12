using Events.Application.Exceptions;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.DeleteUserUseCase.Implementations;

internal class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly UserManager<Member> userManager;

    public DeleteUserUseCase(UserManager<Member> userManager)
    {
        this.userManager = userManager;
    }

    public async Task Execute(string userName)
    {
        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        await userManager.DeleteAsync(user);
    }
}
