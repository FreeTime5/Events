using Events.Application.Models.Account;
using Events.Application.Services.Account;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Events.Application.Services.AccountService.Implementations
{
    internal class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly IValidator<LogInRequestDTO> logInValidator;
        private readonly IValidator<RegisterRequestDTO> registerValidator;
        private readonly IConfiguration configuration;

        public AccountService(UserManager<User> userManager,
            IValidator<LogInRequestDTO> logInValidator,
            IValidator<RegisterRequestDTO> registerValidator,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.logInValidator = logInValidator;
            this.registerValidator = registerValidator;
            this.configuration = configuration;
        }

        public async Task<User?> GetUser(ClaimsPrincipal claims)
        {
            return await userManager.FindByNameAsync(claims.Identity.Name);
        }

        public async Task<LogInResoponseDTO> LogIn(LogInRequestDTO requestDTO)
        {
            var validateResult = await logInValidator.ValidateAsync(requestDTO);

            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var user = await userManager.FindByNameAsync(requestDTO.UserName);

            if (user == null)
            {
                throw new ItemNotFoundException("User");
            }

            if (!await userManager.CheckPasswordAsync(user, requestDTO.Password))
            {
                throw new Exception("Incorrect password");
            }

            return await GenerateLogInResponse(user);
        }

        public async Task<LogInResoponseDTO> LogOut(ClaimsPrincipal claims)
        {
            var user = await userManager.FindByNameAsync(claims.Identity.Name);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = DateTime.UtcNow;

                await userManager.UpdateAsync(user);
            }

            return new LogInResoponseDTO();
        }

        public async Task<LogInResoponseDTO> RefreshToken(RefreshTokenRequestDTO requestDTO)
        {
            var principal = GetTokenPrincipal(requestDTO.JwtToken);

            if (principal?.Identity?.Name == null)
            {
                throw new UserNotSignedInException();
            }

            var user = await userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null || user.RefreshToken == null || user.RefreshToken != requestDTO.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new UserNotSignedInException();
            }

            return await GenerateLogInResponse(user);
        }

        public async Task<LogInResoponseDTO> RegisterUserAndSignIn(RegisterRequestDTO requestDTO)
        {
            var validateResult = await registerValidator.ValidateAsync(requestDTO);

            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var user = new User()
            {
                Email = requestDTO.Email,
                UserName = requestDTO.UserName,
            };

            var result = await userManager.CreateAsync(user, requestDTO.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Can not create user");
            }
            await userManager.AddToRoleAsync(user, "User");

            var response = await LogIn(new LogInRequestDTO() { UserName = requestDTO.UserName, Password = requestDTO.Password });

            return response;
        }

        private async Task<LogInResoponseDTO> GenerateLogInResponse(User user)
        {
            var response = new LogInResoponseDTO()
            {
                IsLogedIn = true,
                JwtToken = GenerateTokenString(user.UserName),
                RefreshToken = GenerateRefreshToken()
            };

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);
            await userManager.UpdateAsync(user);

            return response;
        }

        private ClaimsPrincipal GetTokenPrincipal(string jwtToken)
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

            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validation, out _);
        }

        private string GenerateTokenString(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "User")
            };

            var staticKey = configuration.GetSection("Jwt:Key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(staticKey));
            var signingCard = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddSeconds(60), signingCredentials: signingCard);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        public async Task DeleteUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                throw new ItemNotFoundException("User");
            }

            await userManager.DeleteAsync(user);
        }
    }
}
