using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

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
        if (string.IsNullOrWhiteSpace(_session.UserId))
            throw new UserInputException("You must be logged in to view saved articles.");

        return _apiClient.GetAsync<List<ArticleDto>>($"/api/savedArticle?userId={_session.UserId}");
    }

    public Task SaveArticleAsync(int articleId)
    {
        if (string.IsNullOrWhiteSpace(_session.UserId))
            throw new UserInputException("You must be logged in to save an article.");

        if (articleId <= 0)
            throw new ValidationException("Invalid Article ID. Must be a positive number.");

        var payload = new { articleId, userId = _session.UserId };
        return _apiClient.PostAsync("/api/savedArticle", payload);
    }

    public Task<bool> DeleteSavedArticleAsync(int articleId)
    {
        if (string.IsNullOrWhiteSpace(_session.UserId))
            throw new UserInputException("You must be logged in to delete a saved article.");

        if (articleId <= 0)
            throw new ValidationException("Invalid Article ID. Must be a positive number.");

        return _apiClient.DeleteAsync($"/api/savedArticle?userId={_session.UserId}&articleId={articleId}");
    }
}
