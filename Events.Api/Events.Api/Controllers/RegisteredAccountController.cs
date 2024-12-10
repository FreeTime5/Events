using Events.Api.ApiServices.CookieService;
using Events.Api.Filters;
using Events.Application.Models.Account;
using Events.Application.Services.ClaimsService;
using Events.Application.UseCases.AccountUseCases.GetUserUseCase;
using Events.Application.UseCases.AccountUseCases.RegisterUserAndLoginUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisteredAccountController : Controller
{
    private readonly IGetUserUseCase getUserUseCase;
    private readonly IRegisterUserAndLoginUseCase registerUserAndLoginUseCase;
    private readonly IClaimsService claimsService;
    private readonly ICookieService cookieService;

    public RegisteredAccountController(IGetUserUseCase getUserUseCase,
        IRegisterUserAndLoginUseCase registerUserAndLoginUseCase,
        IClaimsService claimsService,
        ICookieService cookieService)
    {
        this.getUserUseCase = getUserUseCase;
        this.registerUserAndLoginUseCase = registerUserAndLoginUseCase;
        this.claimsService = claimsService;
        this.cookieService = cookieService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userName = claimsService.GetName(User);

        var user = await getUserUseCase.Execute(userName);

        return Ok(user.UserName);
    }

    [HttpPost]
    [Route("[action]")]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> Add([FromBody] RegisterRequestDTO requestDTO)
    {
        var loginResult = await registerUserAndLoginUseCase.Execute(requestDTO);
        cookieService.SetAuthorizationCookies(loginResult);

        return Ok(loginResult);
    }
}
