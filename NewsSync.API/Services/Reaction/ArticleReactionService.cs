using NewsSync.API.Models.Domain;
using NewsSync.API.Models.DTO;
using NewsSync.API.Repositories;

namespace NewsSync.API.Services
{
    public class ArticleReactionService : IArticleReactionService
    {
        private readonly IArticleReactionRepository _reactionRepo;

        public ArticleReactionService(IArticleReactionRepository reactionRepo)
        {
            _reactionRepo = reactionRepo;
        }

        public async Task SubmitReactionAsync(ReactionRequestDto dto)
        {
            await _reactionRepo.AddOrUpdateReactionAsync(dto);
        }

        public async Task<List<Article>> GetReactionsForUserAsync(string userId, bool? isLiked = null)
        {
            var reactions = await _reactionRepo.GetUserReactionsAsync(userId, isLiked);
            return reactions.Select(r => r.Article).ToList();
        }
    }
}