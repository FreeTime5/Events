using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.GetEventsWithPaginationUseCase;

public interface IGetEventsWithPaginationUseCase
{
    IEnumerable<GetEventsResponseDTO> Execute(int page);
}
