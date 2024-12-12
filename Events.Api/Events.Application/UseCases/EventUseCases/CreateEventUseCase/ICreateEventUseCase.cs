using Events.Application.Models.Event;

namespace Events.Application.UseCases.EventUseCases.CreateEventUseCase;

public interface ICreateEventUseCase
{
    Task Execute(CreateEventRequestDTO eventRequestDTO, string userName);
}
