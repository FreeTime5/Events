using Events.Domain.Entities;

namespace Events.Infrastructure.Services.EmailService;

public interface IEmailService
{
    Task SendEmail(IEnumerable<Member> users, string subject, string message);
}
