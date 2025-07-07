using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class ArticleInteractionServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<ISessionContext> _sessionMock;
        private readonly ArticleInteractionService _service;

        public ArticleInteractionServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _sessionMock = new Mock<ISessionContext>();
            _service = new ArticleInteractionService(_apiClientMock.Object, _sessionMock.Object);
        }

        [Fact]
        public async Task FetchHeadlinesAsync_ShouldThrow_WhenFromDateIsAfterToDate()
        {
            var from = DateTime.Today;
            var to = DateTime.Today.AddDays(-1);

            Func<Task> act = async () => await _service.FetchHeadlinesAsync(from, to);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Start date must be earlier than end date.");
        }

        [Fact]
        public async Task SaveArticleAsync_ShouldThrow_WhenArticleIdIsInvalid()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user1");

            Func<Task> act = async () => await _service.SaveArticleAsync(0);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Invalid article ID.");
        }

        [Fact]
        public async Task SaveArticleAsync_ShouldThrow_WhenUserNotLoggedIn()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.SaveArticleAsync(1);

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to save articles.");
        }

        [Fact]
        public async Task ReactToArticleAsync_ShouldThrow_WhenInvalidArticleId()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user1");

            Func<Task> act = async () => await _service.ReactToArticleAsync(0, true);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Invalid article ID.");
        }

        [Fact]
        public async Task ReactToArticleAsync_ShouldThrow_WhenUserNotLoggedIn()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.ReactToArticleAsync(1, false);

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to react to articles.");
        }

        [Fact]
        public async Task ReportArticleAsync_ShouldThrow_WhenReasonIsEmpty()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user1");

            Func<Task> act = async () => await _service.ReportArticleAsync(5, "");

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Report reason cannot be empty.");
        }

        [Fact]
        public async Task GetUserReactionsAsync_ShouldReturnEmpty_WhenUserNotLoggedIn()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            var result = await _service.GetUserReactionsAsync(true);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task FetchCategoriesAsync_ShouldCallApi()
        {
            var categories = new List<CategoryDto> { new CategoryDto() };
            _apiClientMock
                .Setup(api => api.GetAsync<List<CategoryDto>>("/api/categories/article"))
                .ReturnsAsync(categories);

            var result = await _service.FetchCategoriesAsync();

            result.Should().HaveCount(1);
        }
    }
}
