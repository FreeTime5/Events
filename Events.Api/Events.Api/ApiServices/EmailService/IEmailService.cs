namespace Events.Api.ApiServices.EmailService;

public interface IEmailService
{
    Task SendEmail(string email, string subject, string message);
}
