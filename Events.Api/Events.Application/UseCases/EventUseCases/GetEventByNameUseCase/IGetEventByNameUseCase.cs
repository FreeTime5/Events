
using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.GetEventByNameUseCase;

public interface IGetEventByNameUseCase
{
    Task<GetEventsResponseDTO> Execute(string name);
}
