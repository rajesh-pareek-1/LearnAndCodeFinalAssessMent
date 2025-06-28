using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public interface IArticleReactionRepository
    {
        Task AddOrUpdateReactionAsync(ReactionRequestDto dto);
        Task<List<ArticleReaction>> GetUserReactionsAsync(string userId, bool? isLiked = null);
    }

}