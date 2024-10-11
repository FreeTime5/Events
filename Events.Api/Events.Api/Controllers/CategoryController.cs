using Events.Api.Filters;
using Events.Application.Services.CategoryService;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly UserManager<User> userManager;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager)
        {
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [Route("Add")]
        [HttpPost]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ItemNotFoundException("User");
            }

            await categoryService.AddCategory(name, user);

            return Ok();
        }

        [Route("Remove")]
        [HttpDelete]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Delete([FromBody] string name)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ItemNotFoundException("User");
            }

            await categoryService.DeleteCategory(name, user);
            return Ok();
        }

        [Route("List")]
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = (await categoryService.GetAllCategories())
                .Select(c => c.Name);

            return Ok(categories);
        }
    }
}
