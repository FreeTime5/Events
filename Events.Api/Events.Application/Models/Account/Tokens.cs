
namespace Events.Application.Models.Account;

public class Tokens
{
    public string? JwtToken { get; set; }

    public string? RefreshToken { get; set; }

    public RefreshTokenRequestDTO GetRefreshTokenRequestDTO()
    {
        return new RefreshTokenRequestDTO() { JwtToken = this.JwtToken, RefreshToken = this.RefreshToken };
    }
}
