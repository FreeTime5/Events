namespace Events.Application.Servicies.AccountService.DTOs;

public class LogInRequestDTO
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
