namespace Events.Application.Models.Account;

public class RefreshTokenRequestDTO
{
    public string JwtToken { get; set; }

    public string RefreshToken { get; set; }
}
