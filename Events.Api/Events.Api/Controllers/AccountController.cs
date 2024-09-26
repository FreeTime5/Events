using Events.Application.Interfaces;
using Events.Application.Servicies.AccountService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO)
        {
            var registerResult = await _accountService.RegisterUserAndSignIn(requestDTO);
            if (!registerResult.Secceeded)
                return BadRequest(registerResult);

            return Ok(registerResult);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("MyEvents", "Events");
            }

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> LogOut()
        {
            await _accountService.LogOut();

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LogIn([FromBody] LogInRequestDTO requestDTO)
        {
            var result = await _accountService.LogIn(requestDTO);

            if (!result.Secceeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
