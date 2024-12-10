using AutoMapper;
using Events.Application.Exceptions;
using Events.Application.Models.Member;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.MemberUseCases.GetMembersOfEventUseCase.Implementations;

internal class GetMembersOfEventUseCase : IGetMembersOfEventUseCase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetMembersOfEventUseCase(IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        unitOfWork.SetEventRepository();
        unitOfWork.SetRegistrationRepository();
    }

    public async Task<IEnumerable<GetMemberDTO>> Execute(string eventId)
    {
        var eventEntity = await unitOfWork.EventRepsository
            .FindById(eventId)
            ?? throw new ItemNotFoundException("Event");

        var membersOfEvent = unitOfWork.RegistrationRepository
            .FindByAsNoTracking(r => r.EventId == eventId)
            .Select(r => r.Member);

        return membersOfEvent.Select(mapper.Map<GetMemberDTO>);
    }
}
