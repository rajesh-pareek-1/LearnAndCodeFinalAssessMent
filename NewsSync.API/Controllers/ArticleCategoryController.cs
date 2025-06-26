using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/categories/article")]
    [Authorize(Roles = "User,Admin")]
    public class ArticleCategoryController : ControllerBase
    {
        private readonly IArticleCategoryService _articleCategoryService;

        public ArticleCategoryController(IArticleCategoryService _articleCategoryService)
        {
            this._articleCategoryService = _articleCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _articleCategoryService.GetAllCategoriesAsync();

            var response = categories.Select(c => new
            {
                c.Id,
                c.Name,
                c.Description
            });

            return Ok(response);
        }
    }
}
