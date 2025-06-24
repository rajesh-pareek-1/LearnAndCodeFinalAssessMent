using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;

namespace NewsSync.API.Repositories
{
    public interface IArticleReactionRepository
    {
        Task AddOrUpdateReactionAsync(ReactionRequestDto dto);
        Task<List<ArticleReaction>> GetUserReactionsAsync(string userId, bool? isLiked = null);
    }

}