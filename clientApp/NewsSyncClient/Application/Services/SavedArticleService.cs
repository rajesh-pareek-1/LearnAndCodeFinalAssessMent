using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Interfaces;

namespace NewsSyncClient.Application.Services;

public class SavedArticleService : ISavedArticleService
{
    private readonly IApiClient _apiClient;
    private readonly ISessionContext _session;

    public SavedArticleService(IApiClient apiClient, ISessionContext session)
    {
        _apiClient = apiClient;
        _session = session;
    }

    public Task<List<ArticleDto>> GetSavedArticlesAsync()
    {
        if (_session.UserId is null)
            return Task.FromResult(new List<ArticleDto>());

        return _apiClient.GetAsync<List<ArticleDto>>($"/api/savedArticle?userId={_session.UserId}");
    }

    public Task SaveArticleAsync(int articleId)
    {
        var payload = new { articleId, userId = _session.UserId };
        return _apiClient.PostAsync("/api/savedArticle", payload);
    }

    public Task<bool> DeleteSavedArticleAsync(int articleId)
    {
        if (_session.UserId is null)
            return Task.FromResult(false);

        return _apiClient.DeleteAsync($"/api/savedArticle?userId={_session.UserId}&articleId={articleId}");
    }
}
