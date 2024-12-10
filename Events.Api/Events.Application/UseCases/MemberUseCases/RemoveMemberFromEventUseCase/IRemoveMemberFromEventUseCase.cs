namespace Events.Application.UseCases.MemberUseCases.RemoveMemberFromEventUseCase;

public interface IRemoveMemberFromEventUseCase
{
    Task Execute(string eventId, string userName);
}
