namespace Events.Application.Models.Account;

public class LogInResoponseDTO
{
    public bool IsLogedIn { get; set; } = false;

    public string JwtToken { get; set; }

    public string RefreshToken { get; set; }
}
