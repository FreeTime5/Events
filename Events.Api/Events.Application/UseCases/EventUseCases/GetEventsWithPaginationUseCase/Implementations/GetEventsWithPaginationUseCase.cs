using AutoMapper;
using Events.Application.Models.Event;
using Events.Application.Validators.Event;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.EventUseCases.GetEventsWithPaginationUseCase.Implementations;

internal class GetEventsWithPaginationUseCase : IGetEventsWithPaginationUseCase
{
    private const int EVENTSONPAGE = 8;
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly PageValidator pageValidator;

    public GetEventsWithPaginationUseCase(IMapper mapper,
        IUnitOfWork unitOfWork,
        PageValidator pageValidator)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        this.pageValidator = pageValidator;
        unitOfWork.SetEventRepository();
    }

    public IEnumerable<GetEventsResponseDTO> Execute(int page)
    {
        if (!pageValidator.Validate(page).IsValid)
        {
            throw new InvalidDataException("Page must be 1 or greater");
        }

        var events = unitOfWork.EventRepsository.GetEventsWithPagination(page, EVENTSONPAGE);

        return events.Select(mapper.Map<GetEventsResponseDTO>);
    }
}
