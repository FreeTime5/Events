using Events.Application.Services.EmailService;
using Events.Domain.Entities;
using System.Net;
using System.Net.Mail;

namespace Events.Infrastructure.Services.EmailService;

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

    public async Task SendEmail(IEnumerable<Member> users, string subject, string message)
    {
        foreach (var user in users)
        {
            var email = user.Email;

            if (string.IsNullOrEmpty(email))
            {
                continue;
            }

            var client = new SmtpClient(serverHost, 587)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            await client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
    }
}
