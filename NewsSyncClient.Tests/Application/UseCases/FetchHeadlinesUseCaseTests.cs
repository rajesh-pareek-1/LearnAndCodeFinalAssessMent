using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Models.Articles;
using NewsSyncClient.Core.Models.Categories;
using Xunit;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class FetchHeadlinesUseCaseTests
    {
        private readonly Mock<IArticleInteractionService> _articleServiceMock;
        private readonly Mock<ISessionContext> _sessionContextMock;
        private readonly FetchHeadlinesUseCase _useCase;

        public FetchHeadlinesUseCaseTests()
        {
            _articleServiceMock = new Mock<IArticleInteractionService>();
            _sessionContextMock = new Mock<ISessionContext>();
            _useCase = new FetchHeadlinesUseCase(_articleServiceMock.Object, _sessionContextMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnArticles_WhenUserNotLoggedIn()
        {
            _sessionContextMock.Setup(x => x.UserId).Returns((string?)null);

            var articles = new List<ArticleDto> { new() { Id = 1 } };
            _articleServiceMock.Setup(x => x.FetchHeadlinesAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                               .ReturnsAsync(articles);

            var result = await _useCase.ExecuteAsync(DateTime.Now.AddDays(-1), DateTime.Now);

            Assert.Single(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldMapLikesAndDislikes_WhenUserLoggedIn()
        {
            _sessionContextMock.Setup(x => x.UserId).Returns("user123");

            var articles = new List<ArticleDto>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            };

            _articleServiceMock.Setup(x => x.FetchHeadlinesAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                               .ReturnsAsync(articles);

            _articleServiceMock.Setup(x => x.GetUserReactionsAsync(true))
                               .ReturnsAsync(new List<ArticleDto> { new() { Id = 1 } });

            _articleServiceMock.Setup(x => x.GetUserReactionsAsync(false))
                               .ReturnsAsync(new List<ArticleDto> { new() { Id = 2 } });

            var result = await _useCase.ExecuteAsync(DateTime.Now.AddDays(-1), DateTime.Now);

            Assert.True(result.First(x => x.Id == 1).IsLiked);
            Assert.True(result.First(x => x.Id == 2).IsDisliked);
        }

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnCategoryList()
        {
            var categories = new List<CategoryDto> { new() { Id = 1, Name = "Tech" } };

            _articleServiceMock.Setup(x => x.FetchCategoriesAsync()).ReturnsAsync(categories);

            var result = await _useCase.GetCategoriesAsync();

            Assert.Single(result);
        }
    }
}
