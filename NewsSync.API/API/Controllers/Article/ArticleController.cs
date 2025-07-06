using AutoMapper;
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
        private readonly IMapper mapper;

        public ArticleController(IArticleService articleService, IArticleReactionService reactionService, IMapper mapper)
        {
            this.articleService = articleService;
            this.reactionService = reactionService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? query, [FromQuery] string? userId)
        {
            var articles = await articleService.GetFilteredArticlesAsync(fromDate, toDate, query, userId);
            return Ok(mapper.Map<List<ArticleResponseDto>>(articles));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query, [FromQuery] string? userId)
        {
            var articles = await articleService.SearchArticlesAsync(query, userId);
            return Ok(mapper.Map<List<ArticleResponseDto>>(articles));
        }

        [HttpPost("report")]
        public async Task<IActionResult> ReportArticle([FromBody] ReportDto reportDto)
        {
            await articleService.SubmitReportAsync(reportDto);
            return Ok(ValidationMessages.ReportSubmitted);
        }

        [HttpPost("reaction")]
        public async Task<IActionResult> ReactToArticle([FromBody] ReactionRequestDto reactionRequestDto)
        {
            await reactionService.SubmitReactionAsync(reactionRequestDto);
            return Ok(ValidationMessages.ReactionSubmitted);
        }

        [HttpGet("reaction/user/{userId}")]
        public async Task<IActionResult> GetUserReactions([FromRoute] string userId, [FromQuery] bool? liked = null)
        {
            var articles = await reactionService.GetReactionsForUserAsync(userId, liked);
            return Ok(mapper.Map<List<ArticleResponseDto>>(articles));
        }
    }
}
