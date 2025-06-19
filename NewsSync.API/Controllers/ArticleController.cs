using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? query)
        {
            var articles = await articleService.GetFilteredArticlesAsync(fromDate, toDate, query);
            return Ok(articles);
        }

         //GET /api/article/search?query=your-search-term
    [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            var results = await articleService.SearchArticlesAsync(query);
            return Ok(results);
        }
    }
}
