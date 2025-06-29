using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Application.DTOs;
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
        private readonly IMapper mapper;

        public ArticleCategoryController(IArticleCategoryService articleCategoryService, IMapper mapper)
        {
            this.articleCategoryService = articleCategoryService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await articleCategoryService.GetAllCategoriesAsync();
            return Ok(mapper.Map<List<CategoryResponseDto>>(categories));
        }
    }
}
