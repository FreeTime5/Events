namespace Events.Domain.Entities;

public class Registration
{
    public string Id { get; set; }

    public User User { get; set; }

    public string UserId { get; set; }

    public Event Event { get; set; }

    public string EventId { get; set; }

    public DateTime RegistrationDate { get; set; }
}
