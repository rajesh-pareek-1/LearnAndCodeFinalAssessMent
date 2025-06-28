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

        public SavedArticleController(ISavedArticleService savedArticleService)
        {
            this.savedArticleService = savedArticleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSavedArticles([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(ValidationMessages.UserIdRequired);

            var articles = await savedArticleService.GetSavedArticlesForUserAsync(userId);
            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult> SaveArticle([FromBody] SaveArticleRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || request.ArticleId <= 0)
                return BadRequest(ValidationMessages.InvalidInput);

            var isSaved = await savedArticleService.SaveArticleAsync(request.UserId, request.ArticleId);

            if (!isSaved)
                return Conflict(ValidationMessages.ArticleAlreadySavedOrNotFound);

            return Ok(ValidationMessages.ArticleSavedSuccess);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSavedArticle([FromQuery] string userId, [FromQuery] int articleId)
        {
            if (string.IsNullOrWhiteSpace(userId) || articleId <= 0)
                return BadRequest(ValidationMessages.InvalidInput);

            var isDeleted = await savedArticleService.DeleteSavedArticleAsync(userId, articleId);

            if (!isDeleted)
                return NotFound(ValidationMessages.SavedArticleNotFound);

            return Ok(ValidationMessages.SavedArticleDeletedSuccess);
        }
    }
}
