using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Articles;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class SavedArticleServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<ISessionContext> _sessionMock;
        private readonly SavedArticleService _service;

        public SavedArticleServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _sessionMock = new Mock<ISessionContext>();
            _service = new SavedArticleService(_apiClientMock.Object, _sessionMock.Object);
        }

        [Fact]
        public async Task GetSavedArticlesAsync_ShouldThrow_WhenUserIdMissing()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.GetSavedArticlesAsync();

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to view saved articles.");
        }

        [Fact]
        public async Task GetSavedArticlesAsync_ShouldReturnArticles_WhenUserIdPresent()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");
            _apiClientMock
                .Setup(api => api.GetAsync<List<ArticleDto>>("/api/savedArticle?userId=user123"))
                .ReturnsAsync(new List<ArticleDto> { new ArticleDto() });

            var result = await _service.GetSavedArticlesAsync();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveArticleAsync_ShouldThrow_WhenUserIdMissing()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.SaveArticleAsync(5);

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to save an article.");
        }

        [Fact]
        public async Task SaveArticleAsync_ShouldThrow_WhenArticleIdInvalid()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");

            Func<Task> act = async () => await _service.SaveArticleAsync(0);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Invalid Article ID. Must be a positive number.");
        }

        [Fact]
        public async Task DeleteSavedArticleAsync_ShouldThrow_WhenUserIdMissing()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.DeleteSavedArticleAsync(1);

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to delete a saved article.");
        }

        [Fact]
        public async Task DeleteSavedArticleAsync_ShouldThrow_WhenArticleIdInvalid()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");

            Func<Task> act = async () => await _service.DeleteSavedArticleAsync(0);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Invalid Article ID. Must be a positive number.");
        }

        [Fact]
        public async Task DeleteSavedArticleAsync_ShouldCallApi_WhenValidInput()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");

            _apiClientMock
                .Setup(api => api.DeleteAsync("/api/savedArticle?userId=user123&articleId=5"))
                .ReturnsAsync(true);

            var result = await _service.DeleteSavedArticleAsync(5);

            result.Should().BeTrue();
        }
    }
}
