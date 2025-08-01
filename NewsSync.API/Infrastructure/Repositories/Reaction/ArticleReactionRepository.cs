using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Infrastructure.Repositories
{
    public class ArticleReactionRepository : IArticleReactionRepository
    {
        private readonly NewsSyncNewsDbContext newsDb;

        public ArticleReactionRepository(NewsSyncNewsDbContext newsDb)
        {
            this.newsDb = newsDb;
        }

        public async Task AddOrUpdateReactionAsync(ReactionRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existingReaction = await newsDb.ArticleReactions
                .FirstOrDefaultAsync(r => r.ArticleId == dto.ArticleId && r.UserId == dto.UserId);

            if (existingReaction != null)
            {
                if (existingReaction.IsLiked == dto.IsLiked)
                {
                    newsDb.ArticleReactions.Remove(existingReaction);
                }
                else
                {
                    existingReaction.IsLiked = dto.IsLiked;
                    existingReaction.ReactedAt = DateTime.UtcNow;
                }
            }
            else
            {
                var newReaction = new ArticleReaction
                {
                    ArticleId = dto.ArticleId,
                    UserId = dto.UserId,
                    IsLiked = dto.IsLiked,
                    ReactedAt = DateTime.UtcNow
                };

                await newsDb.ArticleReactions.AddAsync(newReaction);
            }

            await newsDb.SaveChangesAsync();
        }

        public async Task<List<ArticleReaction>> GetUserReactionsAsync(string userId, bool? isLiked = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            var query = newsDb.ArticleReactions
                .Include(r => r.Article)
                .Where(r => r.UserId == userId);

            if (isLiked.HasValue)
                query = query.Where(r => r.IsLiked == isLiked.Value);

            return await query.OrderByDescending(r => r.ReactedAt).ToListAsync();
        }
    }
}
