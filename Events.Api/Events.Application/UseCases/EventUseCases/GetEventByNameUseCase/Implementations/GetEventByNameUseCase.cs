using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.EventUseCases.GetEventByNameUseCase.Implementations;

internal class GetEventByNameUseCase : IGetEventByNameUseCase
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;

    public GetEventByNameUseCase(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
    }

    public async Task<GetEventsResponseDTO> Execute(string name)
    {
        var eventEntity = await unitOfWork.EventRepsository
            .FirstOrDefaultAsNoTracking(e => e.Title == name)
            ?? throw new ItemNotFoundException("Event");

        return mapper.Map<GetEventsResponseDTO>(eventEntity);
    }
}
