namespace Events.Domain.Entities;

public class Registration
{
    public Guid Id { get; set; }

    public User User { get; set; }

    public Event Event { get; set; }

    public DateTime RegistrationDate { get; set; }
}
