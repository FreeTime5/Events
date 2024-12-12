namespace Events.Application.UseCases.EventUseCases.DeleteEventUseCase;

public interface IDeleteEventUseCase
{
    Task Execute(string eventId, string userName);
}
