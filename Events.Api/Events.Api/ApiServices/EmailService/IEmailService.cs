using Events.Application.Models.Event;

namespace Events.Api.ApiServices.EmailService;

public interface IEmailService
{
    Task SendEmail(IEnumerable<GetAllUsersResponseDTO> users, string subject, string message);
}
