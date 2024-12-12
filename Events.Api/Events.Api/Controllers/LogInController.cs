using Events.Api.ApiServices.CookieService;
using Events.Api.Filters;
using Events.Application.Models.Account;
using Events.Application.Services.ClaimsService;
using Events.Application.UseCases.AccountUseCases.CheckIsLoginUseCase;
using Events.Application.UseCases.AccountUseCases.LoginUseCase;
using Events.Application.UseCases.AccountUseCases.LogoutUseCase;
using Events.Application.UseCases.AccountUseCases.RefreshTokenUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogInController : Controller
    {
        private readonly ILogoutUseCase logoutUseCase;
        private readonly ILoginUseCase loginUseCase;
        private readonly ICheckIsLoginUseCase checkIsLoginUseCase;
        private readonly IRefreshTokenUseCase refreshTokenUseCase;
        private readonly ICookieService cookieService;
        private readonly IClaimsService claimsService;

        public LogInController(ILogoutUseCase logoutUseCase,
            ILoginUseCase loginUseCase,
            ICheckIsLoginUseCase checkIsLoginUseCase,
            IRefreshTokenUseCase refreshTokenUseCase,
            ICookieService cookieService,
            IClaimsService claimsService)
        {
            this.logoutUseCase = logoutUseCase;
            this.loginUseCase = loginUseCase;
            this.checkIsLoginUseCase = checkIsLoginUseCase;
            this.refreshTokenUseCase = refreshTokenUseCase;
            this.cookieService = cookieService;
            this.claimsService = claimsService;
        }

        [Authorize(Policy = "RolePolicy")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> LogOut()
        {
            var userName = claimsService.GetName(User);

            var response = await logoutUseCase.Execute(userName);
            cookieService.DeleteAuthorizationCookies();

            return Ok(response);
        }

        [Authorize(Policy = "RolePolicy")]
        [HttpGet]
        public async Task<IActionResult> IsLoginAsync()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault() ?? cookieService.GetAuthorizatoinCookies().JwtToken;
            var userName = claimsService.GetName(User);
            var response = await checkIsLoginUseCase.Execute(accessToken!, userName);

            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Login([FromBody] LogInRequestDTO requestDTO)
        {
            var loginResult = await loginUseCase.Execute(requestDTO);
            cookieService.SetAuthorizationCookies(loginResult);

            return Ok(loginResult);
        }

        [HttpPut]
        [Route("[action]")]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO requestDTO)
        {
            requestDTO = cookieService.GetAuthorizatoinCookies() ?? requestDTO;
            var loginResult = await refreshTokenUseCase.Execute(requestDTO);
            cookieService.SetAuthorizationCookies(loginResult);

            return Ok(loginResult);
        }
    }
}
