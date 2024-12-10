using Events.Api.Filters;
using Events.Application.Services.ClaimsService;
using Events.Application.UseCases.CategoryUseCases.AddCategoryUseCase;
using Events.Application.UseCases.CategoryUseCases.DeleteCategoryUseCase;
using Events.Application.UseCases.CategoryUseCases.GetAllCategoriesUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly IAddCategoryUseCase addCategory;
        private readonly IDeleteCategoryUseCase deleteCategory;
        private readonly IGetAllCategoriesUseCase getAllCategories;
        private readonly IClaimsService claimsService;

        public CategoryController(IAddCategoryUseCase addCategory,
            IDeleteCategoryUseCase deleteCategory,
            IGetAllCategoriesUseCase getAllCategories,
            IClaimsService claimsService)
        {
            this.addCategory = addCategory;
            this.deleteCategory = deleteCategory;
            this.getAllCategories = getAllCategories;
            this.claimsService = claimsService;
        }

        [HttpPost]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            var userName = claimsService.GetName(User);

            await addCategory.Execute(name, userName);

            return Ok();
        }

        [HttpDelete]
        [ServiceFilter(typeof(BindingFilter))]
        public async Task<IActionResult> Delete([FromBody] string name)
        {
            var userName = claimsService.GetName(User);

            await deleteCategory.Execute(name, userName);

            return Ok();
        }

        [Route("List")]
        [HttpGet]
        public IActionResult Categories()
        {
            var categories = getAllCategories.Execute();

            return Ok(categories);
        }
    }
}
