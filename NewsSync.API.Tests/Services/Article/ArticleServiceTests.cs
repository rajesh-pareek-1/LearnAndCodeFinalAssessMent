using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NewsSync.API.Application.Services;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Tests.Services.Article;

public class ArticleServiceTests
{
    private readonly Mock<IArticleRepository> _articleRepoMock = new();
    private readonly Mock<IUserPreferenceRepository> _preferenceRepoMock = new();
    private readonly Mock<ILogger<ArticleService>> _loggerMock = new();
    private readonly ArticleService _service;

    public ArticleServiceTests()
    {
        _service = new ArticleService(_articleRepoMock.Object, _preferenceRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetFilteredArticlesAsync_WithDateRange_FiltersCorrectly()
    {
        // Arrange
        var articles = new List<Article>
        {
            new() { Headline = "Tech News", Description = "AI", PublishedDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"), CategoryId = 1 },
            new() { Headline = "Old News", Description = "History", PublishedDate = DateTime.UtcNow.AddDays(-10).ToString("yyyy-MM-dd"), CategoryId = 2 }
        };

        _articleRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(articles);
        _preferenceRepoMock.Setup(r => r.GetPreferredCategoryIdsAsync(It.IsAny<string>())).ReturnsAsync(new List<int> { 1 });

        var from = DateTime.UtcNow.AddDays(-3);
        var to = DateTime.UtcNow;

        // Act
        var result = await _service.GetFilteredArticlesAsync(from, to, null, "user123");

        // Assert
        result.Should().HaveCount(1);
        result[0].Headline.Should().Be("Tech News");
    }

    [Fact]
    public async Task SearchArticlesAsync_WithMatchingQuery_ReturnsMatchingArticles()
    {
        // Arrange
        var articles = new List<Article>
        {
            new() { Headline = "Breaking AI", Description = "Machine learning", PublishedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"), CategoryId = 1 },
            new() { Headline = "Politics", Description = "Debate", PublishedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"), CategoryId = 2 }
        };

        _articleRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(articles);
        _preferenceRepoMock.Setup(r => r.GetPreferredCategoryIdsAsync("user123")).ReturnsAsync(new List<int> { 1 });

        // Act
        var result = await _service.SearchArticlesAsync("AI", "user123");

        // Assert
        result.Should().ContainSingle(a => a.Headline.Contains("AI"));
    }

    [Fact]
    public async Task SortByUserPreferenceAsync_PrioritizesPreferredCategories()
    {
        // Arrange
        var articles = new List<Article>
        {
            new() { Headline = "Health", CategoryId = 3, PublishedDate = "2025-07-01" },
            new() { Headline = "AI", CategoryId = 1, PublishedDate = "2025-07-02" }
        };

        _preferenceRepoMock.Setup(p => p.GetPreferredCategoryIdsAsync("user123"))
            .ReturnsAsync(new List<int> { 1 });

        // Act
        var sorted = await _service.SortByUserPreferenceAsync(articles, "user123");

        // Assert
        sorted.First().Headline.Should().Be("AI");
    }
}
