using AutoMapper;
using Events.Application.Models.Event;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.EventUseCases.GetAllEventsUseCase.Implementations;

internal class GetAllEventsUseCase : IGetAllEventsUseCase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetAllEventsUseCase(IMapper mapper, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
    }

    public IEnumerable<GetEventsResponseDTO> Execute()
    {
        var events = unitOfWork.EventRepsository
            .GetAllAsNoTracking();

        return events.Select(mapper.Map<GetEventsResponseDTO>);
    }
}
