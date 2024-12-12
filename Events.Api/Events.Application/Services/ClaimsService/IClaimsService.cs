using System.Security.Claims;

namespace Events.Application.Services.ClaimsService;

public interface IClaimsService
{
    string? GetName(ClaimsPrincipal claimsPrincipal);

    string? GetName(string jwtToken);
}
