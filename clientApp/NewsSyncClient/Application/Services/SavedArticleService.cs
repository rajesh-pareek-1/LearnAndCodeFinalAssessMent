using System.Net.Http.Json;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Interfaces;

namespace NewsSyncClient.Application.Services;

public class SavedArticleService : ISavedArticleService
{
    private readonly HttpClient _client;
    private readonly ISessionContext _session;

    public SavedArticleService(IHttpClientProvider clientProvider, ISessionContext session)
    {
        _client = clientProvider.Client;
        _session = session;
    }

    public async Task<List<ArticleDto>> GetSavedArticlesAsync()
    {
        if (_session.UserId is null)
            return [];

        var response = await _client.GetAsync($"/api/savedArticle?userId={_session.UserId}");
        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<ArticleDto>>() ?? [];
    }
    public async Task SaveArticleAsync(int articleId)
    {
        var payload = new { articleId, userId = _session.UserId };
        await _client.PostAsJsonAsync("/api/savedArticle", payload);
    }

    public async Task<bool> DeleteSavedArticleAsync(int articleId)
    {
        if (_session.UserId is null)
            return false;

        var response = await _client.DeleteAsync($"/api/savedArticle?userId={_session.UserId}&articleId={articleId}");
        return response.IsSuccessStatusCode;
    }
}
