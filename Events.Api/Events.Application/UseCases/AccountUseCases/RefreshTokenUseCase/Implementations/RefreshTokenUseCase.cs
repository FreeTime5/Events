using Events.Application.Exceptions;
using Events.Application.Models.Account;
using Events.Application.Services.ClaimsService;
using Events.Application.Services.TokenService;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Events.Application.UseCases.AccountUseCases.RefreshTokenUseCase.Implementations
{
    internal class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly UserManager<Member> userManager;
        private readonly IConfiguration configuration;
        private readonly IClaimsService claimsService;
        private readonly ITokenService tokenService;

        public RefreshTokenUseCase(UserManager<Member> userManager,
            IConfiguration configuration,
            IClaimsService claimsService,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.claimsService = claimsService;
            this.tokenService = tokenService;
        }

        public async Task<LogInResoponseDTO> Execute(RefreshTokenRequestDTO requestDTO)
        {
            var userName = claimsService.GetName(requestDTO.JwtToken) ?? throw new UserNotSignedInException();

            var user = await userManager.FindByNameAsync(userName);

            if (user == null || user.RefreshToken == null || user.RefreshToken != requestDTO.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new UserNotSignedInException();
            }

            return await tokenService.GenerateTokens(user);
        }
    }
}
