using Events.Application.Models.Account;
using Events.Application.UseCases.AccountUseCases.LoginUseCase;
using Events.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.UseCases.AccountUseCases.RegisterUserAndLoginUseCase.Implementations;

internal class RegisterUserAndLoginUseCase : IRegisterUserAndLoginUseCase
{
    private readonly IValidator<RegisterRequestDTO> registerValidator;
    private readonly UserManager<Member> userManager;
    private readonly ILoginUseCase loginUseCase;

    public RegisterUserAndLoginUseCase(IValidator<RegisterRequestDTO> registerValidator,
        UserManager<Member> userManager,
        ILoginUseCase loginUseCase)
    {
        this.registerValidator = registerValidator;
        this.userManager = userManager;
        this.loginUseCase = loginUseCase;
    }

    public async Task<LogInResoponseDTO> Execute(RegisterRequestDTO requestDTO)
    {
        var validatitonResult = await registerValidator.ValidateAsync(requestDTO);

        if (!validatitonResult.IsValid)
        {
            throw new ValidationException(validatitonResult.Errors);
        }

        var user = new Member()
        {
            Email = requestDTO.Email,
            UserName = requestDTO.UserName,
        };

        var result = await userManager.CreateAsync(user, requestDTO.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.First().Description);
        }
        await userManager.AddToRoleAsync(user, "User");

        var response = await loginUseCase.Execute(new LogInRequestDTO() { UserName = requestDTO.UserName, Password = requestDTO.Password });

        return response;
    }
}
