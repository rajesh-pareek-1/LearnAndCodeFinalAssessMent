using Microsoft.AspNetCore.Mvc;
using NewsSync.API.Models.DTOs;
using NewsSync.API.Services;

namespace NewsSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return BadRequest("User ID is required.");

            var articles = await savedArticleService.GetSavedArticlesForUserAsync(userId);
            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult> SaveArticle([FromBody] SaveArticleRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || request.ArticleId <= 0)
                return BadRequest("Invalid input.");

            var result = await savedArticleService.SaveArticleAsync(request.UserId, request.ArticleId);

            if (!result)
                return Conflict("Article already saved or not found.");

            return Ok("Article saved successfully.");
        }

        // Controllers/SavedArticleController.cs
        [HttpDelete]
        public async Task<IActionResult> DeleteSavedArticle(
            [FromQuery] string userId,
            [FromQuery] int articleId)
        {
            if (string.IsNullOrWhiteSpace(userId) || articleId <= 0)
                return BadRequest("Invalid input.");

            var result = await savedArticleService.DeleteSavedArticleAsync(userId, articleId);

            if (!result)
                return NotFound("Saved article not found.");

            return Ok("Saved article deleted successfully.");
        }
    }
}
