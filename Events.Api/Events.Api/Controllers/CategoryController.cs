using Events.Api.Filters;
using Events.Application.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            await categoryService.AddCategory(name, User);

            return Ok();
        }

        [HttpDelete]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Delete([FromBody] string name)
        {
            await categoryService.DeleteCategory(name, User);

            return Ok();
        }

        [Route("List")]
        [HttpGet]
        public IActionResult Categories()
        {
            var categories = categoryService.GetAllCategories()
                .Select(c => c.Name);

            return Ok(categories);
        }
    }
}
