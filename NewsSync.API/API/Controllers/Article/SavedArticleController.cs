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
    public class SavedArticleController : ControllerBase
    {
        private readonly ISavedArticleService savedArticleService;
        private readonly IMapper mapper;

        public SavedArticleController(ISavedArticleService savedArticleService, IMapper mapper)
        {
            this.savedArticleService = savedArticleService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSavedArticles([FromQuery] string userId)
        {
            var articles = await savedArticleService.GetSavedArticlesForUserAsync(userId);
            return Ok(mapper.Map<List<ArticleResponseDto>>(articles));
        }

        [HttpPost]
        public async Task<IActionResult> SaveArticle([FromBody] SaveArticleRequestDto request)
        {
            var isSaved = await savedArticleService.SaveArticleAsync(request.UserId, request.ArticleId);

            if (!isSaved)
                return Conflict(ValidationMessages.ArticleAlreadySavedOrNotFound);

            return Ok(ValidationMessages.ArticleSavedSuccess);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSavedArticle([FromQuery] string userId, [FromQuery] int articleId)
        {
            var isDeleted = await savedArticleService.DeleteSavedArticleAsync(userId, articleId);

            if (!isDeleted)
                return NotFound(ValidationMessages.SavedArticleNotFound);

            return Ok(ValidationMessages.SavedArticleDeletedSuccess);
        }
    }
}
