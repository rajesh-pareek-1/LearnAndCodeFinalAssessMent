using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NewsSync.API.Application.Services;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using System.Collections.Generic;

namespace NewsSync.API.Tests.Services.SavedArticle;

public class SavedArticleServiceTests
{
    private readonly Mock<ISavedArticleRepository> _repoMock = new();
    private readonly Mock<ILogger<SavedArticleService>> _loggerMock = new();
    private readonly SavedArticleService _service;

    public SavedArticleServiceTests()
    {
        _service = new SavedArticleService(_repoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetSavedArticlesForUserAsync_ReturnsArticles()
    {
        var userId = "user123";
        var articles = new List<Article> { new() { Id = 1, Headline = "News A" } };

        _repoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ReturnsAsync(articles);

        var result = await _service.GetSavedArticlesForUserAsync(userId);

        result.Should().BeEquivalentTo(articles);
    }

    [Fact]
    public async Task GetSavedArticlesForUserAsync_ThrowsApplicationException_OnError()
    {
        var userId = "user123";
        _repoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ThrowsAsync(new Exception("DB Error"));

        var act = async () => await _service.GetSavedArticlesForUserAsync(userId);

        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("*Failed to fetch saved articles*");
    }

    [Fact]
    public async Task SaveArticleAsync_ReturnsFalse_WhenArticleDoesNotExist()
    {
        var userId = "user123";
        var articleId = 42;

        _repoMock.Setup(r => r.DoesArticleExistAsync(articleId)).ReturnsAsync(false);

        var result = await _service.SaveArticleAsync(userId, articleId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task SaveArticleAsync_ReturnsFalse_WhenArticleAlreadySaved()
    {
        var userId = "user123";
        var articleId = 42;

        _repoMock.Setup(r => r.DoesArticleExistAsync(articleId)).ReturnsAsync(true);
        _repoMock.Setup(r => r.IsArticleAlreadySavedAsync(userId, articleId)).ReturnsAsync(true);

        var result = await _service.SaveArticleAsync(userId, articleId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task SaveArticleAsync_Saves_WhenValid()
    {
        var userId = "user123";
        var articleId = 42;

        _repoMock.Setup(r => r.DoesArticleExistAsync(articleId)).ReturnsAsync(true);
        _repoMock.Setup(r => r.IsArticleAlreadySavedAsync(userId, articleId)).ReturnsAsync(false);

        var result = await _service.SaveArticleAsync(userId, articleId);

        result.Should().BeTrue();
        _repoMock.Verify(r => r.SaveAsync(userId, articleId), Times.Once);
    }

    [Fact]
    public async Task SaveArticleAsync_ThrowsApplicationException_OnFailure()
    {
        var userId = "user123";
        var articleId = 42;

        _repoMock.Setup(r => r.DoesArticleExistAsync(articleId)).ThrowsAsync(new Exception("Something went wrong"));

        var act = async () => await _service.SaveArticleAsync(userId, articleId);

        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("*Failed to save article*");
    }

    [Fact]
    public async Task DeleteSavedArticleAsync_ReturnsTrue_OnSuccess()
    {
        var userId = "user123";
        var articleId = 42;

        _repoMock.Setup(r => r.DeleteSavedArticleAsync(userId, articleId)).ReturnsAsync(true);

        var result = await _service.DeleteSavedArticleAsync(userId, articleId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteSavedArticleAsync_ThrowsApplicationException_OnFailure()
    {
        var userId = "user123";
        var articleId = 42;

        _repoMock.Setup(r => r.DeleteSavedArticleAsync(userId, articleId)).ThrowsAsync(new Exception("Failure"));

        var act = async () => await _service.DeleteSavedArticleAsync(userId, articleId);

        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("*Failed to delete saved article*");
    }
}
