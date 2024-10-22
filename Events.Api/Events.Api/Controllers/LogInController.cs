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

        [HttpPost]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> LogIn([FromBody] LogInRequestDTO requestDTO)
        {
            var loginResult = await accountService.LogIn(requestDTO);

            if (loginResult.IsLogedIn)
            {
                cookieService.SetAuthorizationCookies(loginResult);

                return Ok(loginResult);
            }

            return Unauthorized();
        }


        [HttpPut]
        [Route("[action]")]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> RefreshToken()
        {
            var tokens = cookieService.GetAuthorizatoinCookies();

            var requestDTO = new RefreshTokenRequestDTO() { JwtToken = tokens.JwtToken, RefreshToken = tokens.RefreshToken };

            var loginResult = await accountService.RefreshToken(requestDTO);

            if (loginResult.IsLogedIn)
            {
                cookieService.SetAuthorizationCookies(loginResult);

                return Ok(loginResult);
            }

            return Unauthorized();
        }
    }
}
