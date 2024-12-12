using Events.Domain.Entities;

namespace Events.Application.Services.EmailService;

public interface IEmailService
{
    Task SendEmail(IEnumerable<Member> users, string subject, string message);
}
