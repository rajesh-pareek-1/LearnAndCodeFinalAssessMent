using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.DTOs;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface IArticleReactionService
    {
        Task SubmitReactionAsync(ReactionRequestDto dto);
        Task<List<Article>> GetReactionsForUserAsync(string userId, bool? isLiked = null);
    }

}