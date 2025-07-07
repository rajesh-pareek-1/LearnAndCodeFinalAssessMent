using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Services;
using NewsSync.API.Domain.Entities;
using Xunit;

namespace NewsSync.API.Tests.Services.Adapter
{
    public class NewsApiOrgClientAdapterTests
    {
        [Fact]
        public async Task FetchArticlesAsync_ShouldReturnParsedArticles_WhenApiReturnsSuccess()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            var responseContent = JsonSerializer.Serialize(new
            {
                articles = new[]
                {
                    new
                    {
                        title = "Test Headline",
                        description = "Test Description",
                        url = "http://example.com",
                        author = "Test Author",
                        urlToImage = "http://example.com/image.jpg",
                        publishedAt = "2025-07-05T12:00:00Z",
                        source = new { name = "Test Source" }
                    }
                }
            });

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var logger = Mock.Of<ILogger<NewsApiOrgClientAdapter>>();

            var adapter = new NewsApiOrgClientAdapter(httpClient, logger);
            var categories = new List<CategoryResponseDto>
            {
                new() { Id = 1, Name = "General" }
            };

            // Act
            var result = await adapter.FetchArticlesAsync("https://fake-newsapi.org/v2/top-headlines", "fake-key", categories);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result[0].Headline.Should().Be("Test Headline");
            result[0].CategoryId.Should().Be(1); // Because ArticleCategoryPredictor always returns first match
        }

        [Fact]
        public async Task FetchArticlesAsync_ShouldReturnEmptyList_WhenApiFails()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(mockHandler.Object);
            var logger = Mock.Of<ILogger<NewsApiOrgClientAdapter>>();

            var adapter = new NewsApiOrgClientAdapter(httpClient, logger);

            // Act
            var result = await adapter.FetchArticlesAsync("https://fake-url", "fake-key", new List<CategoryResponseDto>());

            // Assert
            result.Should().BeEmpty();
        }
    }
}
