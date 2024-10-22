using Events.Api.ApiServices.CookieService;
using Events.Api.Filters;
using Events.Application.Models.Account;
using Events.Application.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisteredAccountController : Controller
{
    private readonly IAccountService accountService;
    private readonly ICookieService cookieService;

    public RegisteredAccountController(IAccountService accountService, ICookieService cookieService)
    {
        this.accountService = accountService;
        this.cookieService = cookieService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var user = await accountService.GetUser(User);
        return Ok(user.UserName);
    }

    [HttpPost]
    [Route("[action]")]
    [ServiceFilter(typeof(BindingFilter))]
    public async Task<IActionResult> Add([FromBody] RegisterRequestDTO requestDTO)
    {
        var loginResult = await accountService.RegisterUserAndSignIn(requestDTO);
        cookieService.SetAuthorizationCookies(loginResult);

        return Ok(loginResult);
    }
}
