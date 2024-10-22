using Events.Application.Exceptions;
using Events.Application.Models.Account;
using Events.Application.Services.Account;
using Events.Infrastructure.Entities;
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
        private readonly UserManager<MemberDb> userManager;
        private readonly IValidator<LogInRequestDTO> logInValidator;
        private readonly IValidator<RegisterRequestDTO> registerValidator;
        private readonly IConfiguration configuration;

        public AccountService(UserManager<MemberDb> userManager,
            IValidator<LogInRequestDTO> logInValidator,
            IValidator<RegisterRequestDTO> registerValidator,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.logInValidator = logInValidator;
            this.registerValidator = registerValidator;
            this.configuration = configuration;
        }

        public async Task<MemberDb?> GetUser(ClaimsPrincipal claims)
        {
            return await userManager.FindByNameAsync(claims.Identity.Name) ?? throw new ItemNotFoundException("User");
        }

        public async Task<LogInResoponseDTO> LogIn(LogInRequestDTO requestDTO)
        {
            var validateResult = await logInValidator.ValidateAsync(requestDTO);

            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var user = await userManager.FindByNameAsync(requestDTO.UserName) ?? throw new ItemNotFoundException("User");

            if (!await userManager.CheckPasswordAsync(user, requestDTO.Password))
            {
                throw new AuthorizationException("Incorrect password");
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

        public async Task<LogInResoponseDTO> IsLogIn(string accessToken, string userName)
        {
            var loginResponse = new LogInResoponseDTO();

            var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

            loginResponse.RefreshToken = user.RefreshToken;
            loginResponse.JwtToken = accessToken;
            loginResponse.IsLogedIn = true;

            return loginResponse;
        }

        public async Task<LogInResoponseDTO> RefreshToken(RefreshTokenRequestDTO requestDTO)
        {
            var principal = GetTokenPrincipal(requestDTO.JwtToken);

            var userName = principal?.Identity?.Name ?? throw new UserNotSignedInException();

            var user = await userManager.FindByNameAsync(userName);

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

            var user = new MemberDb()
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

        public async Task DeleteUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName) ?? throw new ItemNotFoundException("User");

            await userManager.DeleteAsync(user);
        }

        public async Task AddAdmin(string password)
        {
            var admin = await userManager.FindByNameAsync("Admin");

            if (admin is null)
            {
                var adminUser = new MemberDb()
                {
                    Email = "admin@gmail.com",
                    UserName = "Admin"
                };
                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
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

        private async Task<LogInResoponseDTO> GenerateLogInResponse(MemberDb member)
        {
            var role = (await userManager.GetRolesAsync(member)).First();
            var response = new LogInResoponseDTO()
            {
                IsLogedIn = true,
                JwtToken = GenerateTokenString(member.UserName, role),
                RefreshToken = GenerateRefreshToken()
            };

            member.RefreshToken = response.RefreshToken;
            member.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);
            await userManager.UpdateAsync(member);

            return response;
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

        private string GenerateTokenString(string userName, string role)
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
    }
}
