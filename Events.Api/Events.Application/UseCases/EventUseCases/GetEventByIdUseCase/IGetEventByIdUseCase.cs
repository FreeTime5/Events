using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.GetEventByIdUseCase;

public interface IGetEventByIdUseCase
{
    Task<GetEventsResponseDTO> Execute(string id);
}
