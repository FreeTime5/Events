using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.GetFilteredEventsUseCase;

public interface IGetFilteredEventsUseCase
{
    IEnumerable<GetEventsResponseDTO> Execute(int page, string filterItem, string filterValue);
}
