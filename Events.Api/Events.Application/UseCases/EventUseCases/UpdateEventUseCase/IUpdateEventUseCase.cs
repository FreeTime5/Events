using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.UpdateEventUseCase;

public interface IUpdateEventUseCase
{
    Task Execute(UpdateEventRequestDTO eventRequestDTO, string userName);
}
