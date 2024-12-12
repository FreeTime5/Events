using Events.Application.Exceptions;
using Events.Domain.Entities;
using Events.Domain.UnitOfWorkInterface;

namespace Events.Application.UseCases.EventUseCases.GetAllUsersRegistredOnEventUseCase.Implementations;

internal class GetAllUsersRegistredOnEventUseCase : IGetAllUsersRegistredOnEventUseCase
{
    private readonly IUnitOfWork unitOfWork;

    public GetAllUsersRegistredOnEventUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        unitOfWork.SetEventRepository();
        unitOfWork.SetRegistrationRepository();
    }

    public async Task<IEnumerable<Member>> Execute(string eventId)
    {
        var eventInstance = await unitOfWork.EventRepsository
            .FirstOrDefaultAsNoTracking(e => e.Id == eventId)
            ?? throw new ItemNotFoundException("Event");

        return unitOfWork.RegistrationRepository
            .FindByAsNoTracking(r => r.EventId == eventId)
            .Select(r => r.Member);
    }
}
