using Events.Application.Exceptions;
using Events.Application.Validators.Category;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;
using FluentValidation;

namespace Events.Application.UseCases.CategoryUseCases.GetCategoryByNameUseCase.Implementations;

internal class GetCategoryByNameUseCase : IGetCategoryByNameUseCase
{
    private readonly CategoryNameValidator categoryNameValidator;
    private readonly IUnitOfWork unitOfWork;

    public GetCategoryByNameUseCase(CategoryNameValidator categoryNameValidator,
        IUnitOfWork unitOfWork)
    {
        this.categoryNameValidator = categoryNameValidator;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetCategoryRepository();
    }

    public async Task<Category> Execute(string name)
    {
        var validationResult = categoryNameValidator.Validate(name);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await unitOfWork.CategoryRepository
            .FirstOrDefaultAsNoTracking(c => c.Name == name)
            ?? throw new ItemNotFoundException("Category");
    }
}
