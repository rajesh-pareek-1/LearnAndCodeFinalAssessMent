using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Constants;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/categories/article")]
    [Authorize(Roles = $"{RoleNames.User},{RoleNames.Admin}")]
    public class ArticleCategoryController : ControllerBase
    {
        private readonly IArticleCategoryService articleCategoryService;

        public ArticleCategoryController(IArticleCategoryService articleCategoryService)
        {
            this.articleCategoryService = articleCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await articleCategoryService.GetAllCategoriesAsync();

            var response = categories.Select(category => new
            {
                category.Id,
                category.Name,
                category.Description
            });

            return Ok(response);
        }
    }
}
