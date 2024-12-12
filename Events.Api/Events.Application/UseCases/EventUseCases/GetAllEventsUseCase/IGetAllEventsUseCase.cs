using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.GetAllEventsUseCase;

public interface IGetAllEventsUseCase
{
    IEnumerable<GetEventsResponseDTO> Execute();
}
