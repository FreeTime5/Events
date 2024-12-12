using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.AddAdminUseCase.Implementations;

internal class AddAdminUseCase : IAddAdminUseCase
{
    private readonly UserManager<Member> userManager;

    public AddAdminUseCase(UserManager<Member> userManager)
    {
        this.userManager = userManager;
    }

    public async Task Execute(string password)
    {
        var admin = await userManager.FindByNameAsync("Admin");

        if (admin is null)
        {
            var adminUser = new Member()
            {
                Email = "admin@gmail.com",
                UserName = "Admin"
            };

            await userManager.CreateAsync(adminUser, password);
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
