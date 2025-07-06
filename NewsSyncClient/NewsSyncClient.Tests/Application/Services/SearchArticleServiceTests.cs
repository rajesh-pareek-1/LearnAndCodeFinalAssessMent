using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Articles;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class SearchArticleServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly SearchArticleService _service;

        public SearchArticleServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _service = new SearchArticleService(_apiClientMock.Object);
        }

        [Fact]
        public async Task SearchAsync_ShouldThrow_WhenQueryIsEmpty()
        {
            Func<Task> act = async () => await _service.SearchAsync("");

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Search query cannot be empty.");
        }

        [Fact]
        public async Task SearchAsync_ShouldCallApi_WithEncodedQuery()
        {
            var expected = new List<ArticleDto> { new ArticleDto() };
            _apiClientMock
                .Setup(api => api.GetAsync<List<ArticleDto>>("/api/article/search?query=dotnet"))
                .ReturnsAsync(expected);

            var result = await _service.SearchAsync("dotnet");

            result.Should().HaveCount(1);
        }
    }
}
