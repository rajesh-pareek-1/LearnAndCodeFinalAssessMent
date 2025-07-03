using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Domain.Common.Messages;

namespace NewsSync.API.Application.Services
{
    public class ArticleReactionService : IArticleReactionService
    {
        private readonly IArticleReactionRepository reactionRepository;
        private readonly ILogger<ArticleReactionService> logger;

        public ArticleReactionService(IArticleReactionRepository reactionRepository, ILogger<ArticleReactionService> logger)
        {
            this.reactionRepository = reactionRepository;
            this.logger = logger;
        }

        public async Task SubmitReactionAsync(ReactionRequestDto reactionRequestDto)
        {
            try
            {
                await reactionRepository.AddOrUpdateReactionAsync(reactionRequestDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to submit reaction for user {UserId} on article {ArticleId}", reactionRequestDto.UserId, reactionRequestDto.ArticleId);
                throw new ApplicationException(ValidationMessages.FailedToSubmitReaction, ex);
            }
        }

        public async Task<List<Article>> GetReactionsForUserAsync(string userId, bool? isLiked = null)
        {
            try
            {
                var reactions = await reactionRepository.GetUserReactionsAsync(userId, isLiked);
                return [.. reactions.Select(r => r.Article)];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch reactions for user {UserId} with like filter {IsLiked}", userId, isLiked);
                throw new ApplicationException(ValidationMessages.FailedToFetchReactions, ex);
            }
        }
    }
}
