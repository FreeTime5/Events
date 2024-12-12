using Events.Application.Exceptions;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.CategoryUseCases.DeleteCategoryUseCase.Implementations;

internal class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IUnitOfWork unitOfWork;

    public DeleteCategoryUseCase(UserManager<Member> userManager, IUnitOfWork unitOfWork)
    {
        this.userManager = userManager;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetCategoryRepository();
    }

    public async Task Execute(string name, string userName)
    {
        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new UserHaveNoPermissionException();
        }

        var category = await unitOfWork.CategoryRepository
            .FirstOrDefault(c => c.Name == name)
            ?? throw new ItemNotFoundException("Category");

        unitOfWork.CategoryRepository.Delete(category);
        await unitOfWork.Save();
    }
}
