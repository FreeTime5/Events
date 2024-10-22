using Events.Infrastructure.Entities;

namespace Events.Infrastructure.Repositories.RegistrationRepository;

public interface IRegistrationRepository
{
    Task<RegistrationDb?> Find(string MemberId, string EventId);

    Task Add(RegistrationDb registration);

    Task Remove(RegistrationDb registration);
}
