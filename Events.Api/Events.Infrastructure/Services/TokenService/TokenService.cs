using Events.Application.Models.Account;
using Events.Application.Services.TokenService;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Events.Infrastructure.Services.TokenService;

internal class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    private readonly UserManager<Member> userManager;

    public TokenService(IConfiguration configuration,
        UserManager<Member> userManager)
    {
        this.configuration = configuration;
        this.userManager = userManager;
    }

    public string GenerateJWT(string userName, string role)
    {
        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            };

        var staticKey = configuration.GetSection("Jwt:Key").Value;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(staticKey));
        var signingCard = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(15), signingCredentials: signingCard);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public async Task<LogInResoponseDTO> GenerateTokens(Member user)
    {
        var role = (await userManager.GetRolesAsync(user)).First();

        var response = new LogInResoponseDTO()
        {
            IsLogedIn = true,
            JwtToken = GenerateJWT(user.UserName, role),
            RefreshToken = GenerateRefreshToken()
        };

        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);

        await userManager.UpdateAsync(user);

        return response;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }

    public async Task<LogInResoponseDTO> DeleteTokens(Member user)
    {
        user.RefreshToken = null;
        user.RefreshTokenExpiry = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        return new LogInResoponseDTO();
    }
}
