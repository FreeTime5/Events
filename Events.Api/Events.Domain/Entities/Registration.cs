using Microsoft.Extensions.Logging;

namespace Events.Domain.Entities;

public class Registration
{
    public string Id { get; set; }

    public DateTime RegistrationDate { get; set; }

    public Member Member { get; set; }

    public string MemberId { get; set; }

    public Event Event { get; set; }

    public string EventId { get; set; }
}
