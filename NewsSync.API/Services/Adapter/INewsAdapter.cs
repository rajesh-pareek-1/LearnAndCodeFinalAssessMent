// Services/Adapters/INewsAdapter.cs
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Services
{
    public interface INewsAdapter
    {
        Task<List<Article>> FetchArticlesAsync(string baseUrl, string apiKey);
    }

}