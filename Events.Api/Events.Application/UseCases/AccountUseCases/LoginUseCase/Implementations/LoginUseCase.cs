using Events.Application.Exceptions;
using Events.Application.Models.Account;
using Events.Application.Services.TokenService;
using Events.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.LoginUseCase.Implementations;

internal class LoginUseCase : ILoginUseCase
{
    private readonly IValidator<LogInRequestDTO> loginValidator;
    private readonly UserManager<Member> userManager;
    private readonly ITokenService tokenService;

    public LoginUseCase(IValidator<LogInRequestDTO> loginValidator,
        UserManager<Member> userManager,
        ITokenService tokenService)
    {
        this.loginValidator = loginValidator;
        this.userManager = userManager;
        this.tokenService = tokenService;
    }

    public async Task<LogInResoponseDTO> Execute(LogInRequestDTO requestDTO)
    {
        var validateResult = await loginValidator.ValidateAsync(requestDTO);

        if (!validateResult.IsValid)
        {
            throw new ValidationException(validateResult.Errors);
        }

        var user = await userManager.FindByNameAsync(requestDTO.UserName) ?? throw new ItemNotFoundException("User");

        if (!await userManager.CheckPasswordAsync(user, requestDTO.Password))
        {
            throw new AuthorizationException("Incorrect password");
        }

        return await tokenService.GenerateTokens(user);
    }
}
