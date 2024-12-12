using Events.Application.Exceptions;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.CategoryUseCases.AddCategoryUseCase.Implementations;

internal class AddCategoryUseCase : IAddCategoryUseCase
{
    private readonly UserManager<Member> userManager;
    private readonly IUnitOfWork unitOfWork;

    public AddCategoryUseCase(UserManager<Member> userManager, IUnitOfWork unitOfWork)
    {
        this.userManager = userManager;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetCategoryRepository();
    }

    public async Task Execute(string name, string userName)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidDataException("Category name must not be null or empty");
        }

        var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            throw new UserHaveNoPermissionException();
        }

        var sameCategory = await unitOfWork.CategoryRepository
            .FirstOrDefaultAsNoTracking(c => c.Name == name);

        if (sameCategory != null)
        {
            throw new ItemAlreadyAddedException("Category");
        }

        var category = new Category() { Name = name };

        unitOfWork.CategoryRepository.Add(category);
        await unitOfWork.Save();
    }
}
