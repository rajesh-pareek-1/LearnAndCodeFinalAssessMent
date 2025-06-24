using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models.DTO;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public IArticleReactionService _reactionService { get; }

        public ArticleController(IArticleService _articleService, IArticleReactionService articleReactionService)
        {
            this._articleService = _articleService;
            this._reactionService = articleReactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? query)
        {
            var articles = await _articleService.GetFilteredArticlesAsync(fromDate, toDate, query);
            return Ok(articles);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            var results = await _articleService.SearchArticlesAsync(query);
            return Ok(results);
        }

        [HttpPost("report")]
        public async Task<IActionResult> ReportArticle([FromBody] ReportDto dto)
        {
            await _articleService.SubmitReportAsync(dto);
            return Ok("Report submitted successfully.");
        }

        [HttpPost("reaction")]
        public async Task<IActionResult> ReactToArticle([FromBody] ReactionRequestDto dto)
        {
            await _reactionService.SubmitReactionAsync(dto);
            return Ok("Reaction submitted.");
        }

        [HttpGet("reaction/user/{userId}")]
        public async Task<IActionResult> GetUserReactions(string userId, [FromQuery] bool? liked = null)
        {
            var articles = await _reactionService.GetReactionsForUserAsync(userId, liked);
            return Ok(articles);
        }


    }
}
