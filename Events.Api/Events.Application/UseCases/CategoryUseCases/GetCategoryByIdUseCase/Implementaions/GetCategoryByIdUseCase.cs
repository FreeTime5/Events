using Events.Application.Exceptions;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.CategoryUseCases.GetCategoryByIdUseCase.Implementaions;

internal class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
{
    private readonly IUnitOfWork unitOfWork;

    public GetCategoryByIdUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        unitOfWork.SetCategoryRepository();
    }

    public async Task<Category> Execute(string id)
    {
        return await unitOfWork.CategoryRepository
            .FindById(id)
            ?? throw new ItemNotFoundException("Category");
    }
}
