using Events.Application.Services.ClaimsService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Events.Infrastructure.Services.ClaimsService;

internal class ClaimsService : IClaimsService
{
    private readonly IConfiguration configuration;

    public ClaimsService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string? GetName(string jwtToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value));

        var validation = new TokenValidationParameters()
        {
            IssuerSigningKey = securityKey,
            ValidateLifetime = false,
            ValidateActor = false,
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validation, out _);

        return GetName(claimsPrincipal);
    }

    public string? GetName(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity is null ? null : claimsPrincipal.Identity.Name;
    }
}
