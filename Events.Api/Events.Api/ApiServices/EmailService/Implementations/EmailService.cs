using System.Net.Mail;
using System.Net;

namespace Events.Api.ApiServices.EmailService.Implementations;

internal class EmailService : IEmailService
{
    private readonly string mail;
    private readonly string password;
    private readonly string serverHost;

    public EmailService(string mail, string password, string serverHost)
    {
        this.mail = mail;
        this.password = password;
        this.serverHost = serverHost;
    }

    public async Task SendEmail(string email, string subject, string message)
    {
        var client = new SmtpClient(serverHost, 587)
        {
            UseDefaultCredentials = false,
            EnableSsl = true,
            Credentials = new NetworkCredential(mail, password)
        };

        await client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
    }
}