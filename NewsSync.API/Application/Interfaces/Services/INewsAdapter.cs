using NewsSync.API.Application.DTOs;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Application.Interfaces.Services
{
    public interface INewsAdapter
    {
        Task<List<Article>> FetchArticlesAsync(string baseUrl, string apiKey, List<CategoryResponseDto> categories);
    }
}