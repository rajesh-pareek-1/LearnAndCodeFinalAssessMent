using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Infrastructure.Data;

namespace NewsSync.API.Application.Interfaces.Repositories
{
    public class ArticleReactionRepository : IArticleReactionRepository
    {
        private readonly NewsSyncNewsDbContext _db;

        public ArticleReactionRepository(NewsSyncNewsDbContext db)
        {
            _db = db;
        }

        public async Task AddOrUpdateReactionAsync(ReactionRequestDto dto)
        {
            var existing = await _db.ArticleReactions
                .FirstOrDefaultAsync(r => r.ArticleId == dto.ArticleId && r.UserId == dto.UserId);

            if (existing != null)
            {
                if (existing.IsLiked == dto.IsLiked)
                {
                    // Same reaction submitted again, so remove (toggle off)
                    _db.ArticleReactions.Remove(existing);
                }
                else
                {
                    // Opposite reaction submitted, so update
                    existing.IsLiked = dto.IsLiked;
                    existing.ReactedAt = DateTime.UtcNow;
                }
            }
            else
            {
                // No reaction yet, add new
                await _db.ArticleReactions.AddAsync(new ArticleReaction
                {
                    ArticleId = dto.ArticleId,
                    UserId = dto.UserId,
                    IsLiked = dto.IsLiked,
                    ReactedAt = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }


        public async Task<List<ArticleReaction>> GetUserReactionsAsync(string userId, bool? isLiked = null)
        {
            var query = _db.ArticleReactions
                .Include(r => r.Article)
                .Where(r => r.UserId == userId);

            if (isLiked.HasValue)
                query = query.Where(r => r.IsLiked == isLiked.Value);

            return await query.OrderByDescending(r => r.ReactedAt).ToListAsync();
        }
    }
}