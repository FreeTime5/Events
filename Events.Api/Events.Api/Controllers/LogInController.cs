using Events.Api.ApiServices.CookieService;
using Events.Api.Filters;
using Events.Application.Models.Account;
using Events.Application.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogInController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ICookieService cookieService;

        public LogInController(IAccountService accountService, ICookieService cookieService)
        {
            this.accountService = accountService;
            this.cookieService = cookieService;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> LogOut()
        {
            var response = await accountService.LogOut(User);
            cookieService.DeleteAuthorizationCookies();

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IsLoginAsync()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault() ?? cookieService.GetAuthorizatoinCookies().JwtToken;
            var response = await accountService.IsLogIn(accessToken!, User.Identity!.Name!);

            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Login([FromBody] LogInRequestDTO requestDTO)
        {
            var loginResult = await accountService.LogIn(requestDTO);
            cookieService.SetAuthorizationCookies(loginResult);

            return Ok(loginResult);
        }

        [HttpPut]
        [Route("[action]")]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO requestDTO)
        {
            requestDTO = cookieService.GetAuthorizatoinCookies() ?? requestDTO;
            var loginResult = await accountService.RefreshToken(requestDTO);
            cookieService.SetAuthorizationCookies(loginResult);

            return Ok(loginResult);
        }
    }
}
