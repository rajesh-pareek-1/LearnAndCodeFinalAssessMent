using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Constants;
using NewsSync.API.Domain.Common.Messages;

namespace NewsSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.User)]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService articleService;
        private readonly IArticleReactionService reactionService;

        public ArticleController(IArticleService articleService, IArticleReactionService reactionService)
        {
            this.articleService = articleService;
            this.reactionService = reactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? query)
        {
            var articles = await articleService.GetFilteredArticlesAsync(fromDate, toDate, query);
            return Ok(articles);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(ValidationMessages.SearchQueryRequired);

            var results = await articleService.SearchArticlesAsync(query);
            return Ok(results);
        }

        [HttpPost("report")]
        public async Task<IActionResult> ReportArticle([FromBody] ReportDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.UserId) || dto.ArticleId <= 0)
                return BadRequest(ValidationMessages.InvalidInput);

            await articleService.SubmitReportAsync(dto);
            return Ok(ValidationMessages.ReportSubmitted);
        }

        [HttpPost("reaction")]
        public async Task<IActionResult> ReactToArticle([FromBody] ReactionRequestDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.UserId) || dto.ArticleId <= 0)
                return BadRequest(ValidationMessages.InvalidInput);

            await reactionService.SubmitReactionAsync(dto);
            return Ok(ValidationMessages.ReactionSubmitted);
        }

        [HttpGet("reaction/user/{userId}")]
        public async Task<IActionResult> GetUserReactions([FromRoute] string userId, [FromQuery] bool? liked = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(ValidationMessages.UserIdRequired);

            var articles = await reactionService.GetReactionsForUserAsync(userId, liked);
            return Ok(articles);
        }
    }
}
