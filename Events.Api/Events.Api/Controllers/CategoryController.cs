using Events.Application.Interfaces;
using Events.Application.Models;
using Events.Application.Servicies.ServiciesErrors;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest(Result.Failure([AccountErrors.UserNotSignedIn]));

            var result = await _categoryService.AddCategory(name, user);
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromBody] string name)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest(Result.Failure([AccountErrors.UserNotSignedIn]));

            var result = await _categoryService.DeleteCategory(name, user);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = (await _categoryService.GetAllCategories())
                .Select(c => c.Name);

            return Ok(categories);
        }
    }
}
