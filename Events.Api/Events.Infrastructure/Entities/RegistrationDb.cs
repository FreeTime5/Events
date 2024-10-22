using Events.Domain.Entities;

namespace Events.Infrastructure.Entities;

public class RegistrationDb : Registration
{
    public string Id { get; set; }

    public MemberDb Member { get; set; }

    public string MemberId { get; set; }

    public EventDb Event { get; set; }

    public string EventId { get; set; }
}
