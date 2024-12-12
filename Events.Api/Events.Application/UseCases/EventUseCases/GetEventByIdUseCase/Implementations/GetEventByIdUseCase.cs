using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Event;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.EventUseCases.GetEventByIdUseCase.Implementations;

internal class GetEventByIdUseCase : IGetEventByIdUseCase
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;

    public GetEventByIdUseCase(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
    }

    public async Task<GetEventsResponseDTO> Execute(string id)
    {
        var eventInstance = await unitOfWork.EventRepsository
            .FirstOrDefaultAsNoTracking(e => e.Id == id)
            ?? throw new ItemNotFoundException("Event");

        return mapper.Map<GetEventsResponseDTO>(eventInstance);
    }
}
