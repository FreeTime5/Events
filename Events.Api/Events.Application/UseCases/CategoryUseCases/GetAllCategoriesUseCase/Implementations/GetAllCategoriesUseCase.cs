using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.CategoryUseCases.GetAllCategoriesUseCase.Implementations;

internal class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private readonly IUnitOfWork unitOfWork;

    public GetAllCategoriesUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        unitOfWork.SetCategoryRepository();
    }

    public IEnumerable<Category> Execute()
    {
        return unitOfWork.CategoryRepository.GetAllAsNoTracking();
    }
}
