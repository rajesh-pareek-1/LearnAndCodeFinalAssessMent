using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;

namespace NewsSync.API.Services
{
    public interface IArticleReactionService
    {
        Task SubmitReactionAsync(ReactionRequestDto dto);
        Task<List<Article>> GetReactionsForUserAsync(string userId, bool? isLiked = null);
    }

}