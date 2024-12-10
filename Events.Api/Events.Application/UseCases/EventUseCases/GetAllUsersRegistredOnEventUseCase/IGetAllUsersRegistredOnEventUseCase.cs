using Events.Domain.Entities;

namespace Events.Application.UseCases.EventUseCases.GetAllUsersRegistredOnEventUseCase;

public interface IGetAllUsersRegistredOnEventUseCase
{
    Task<IEnumerable<Member>> Execute(string eventId);
}
