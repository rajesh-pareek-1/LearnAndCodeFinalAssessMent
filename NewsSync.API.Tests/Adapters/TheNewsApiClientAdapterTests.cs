using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Services;
using NewsSync.API.Domain.Entities;
using Xunit;

namespace NewsSync.API.Tests.Adapters;

public class TheNewsApiClientAdapterTests
{
    private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
    private readonly HttpClient httpClient;
    private readonly Mock<ILogger<TheNewsApiClientAdapter>> loggerMock;
    private readonly TheNewsApiClientAdapter adapter;

    public TheNewsApiClientAdapterTests()
    {
        httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.thenewsapi.com/")
        };

        loggerMock = new Mock<ILogger<TheNewsApiClientAdapter>>();
        adapter = new TheNewsApiClientAdapter(httpClient, loggerMock.Object);
    }

    [Fact]
    public async Task FetchArticlesAsync_ReturnsParsedArticles_WhenResponseIsValid()
    {
        // Arrange
        var json = """
        {
          "data": [
            {
              "title": "Test Headline",
              "description": "Test Description",
              "source": "The News API",
              "url": "https://example.com",
              "author": "Author Name",
              "image_url": "https://image.com",
              "language": "en",
              "published_at": "2025-07-05T12:00:00Z"
            }
          ]
        }
        """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        SetupHttpHandler(responseMessage);

        var categories = new List<CategoryResponseDto>
        {
            new() { Id = 1, Name = "General" }
        };

        // Act
        var articles = await adapter.FetchArticlesAsync("https://api.thenewsapi.com/news", "dummy-api-key", categories);

        // Assert
        articles.Should().HaveCount(1);
        articles[0].Headline.Should().Be("Test Headline");
        articles[0].CategoryId.Should().Be(1); // Predicted by mock predictor
    }

    [Fact]
    public async Task FetchArticlesAsync_ReturnsEmptyList_WhenApiFails()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        SetupHttpHandler(responseMessage);

        var categories = new List<CategoryResponseDto>
        {
            new() { Id = 1, Name = "General" }
        };

        // Act
        var result = await adapter.FetchArticlesAsync("https://api.thenewsapi.com/news", "dummy-api-key", categories);

        // Assert
        result.Should().BeEmpty();
    }

    private void SetupHttpHandler(HttpResponseMessage response)
    {
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response)
            .Verifiable();
    }
}
