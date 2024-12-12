namespace Events.Application.UseCases.MemberUseCases.AddMemberToEventUseCase;

public interface IAddMemberToEventUseCase
{
    Task Execute(string eventId, string userName);
}
