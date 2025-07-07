using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;
using Xunit;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class FetchSavedArticlesUseCaseTests
    {
        private readonly Mock<ISavedArticleService> _serviceMock;
        private readonly FetchSavedArticlesUseCase _useCase;

        public FetchSavedArticlesUseCaseTests()
        {
            _serviceMock = new Mock<ISavedArticleService>();
            _useCase = new FetchSavedArticlesUseCase(_serviceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnSavedArticles()
        {
            var articles = new List<ArticleDto>
            {
                new() { Id = 1, Title = "Saved Article" }
            };

            _serviceMock.Setup(s => s.GetSavedArticlesAsync()).ReturnsAsync(articles);

            var result = await _useCase.ExecuteAsync();

            Assert.Equal(articles, result);
        }
    }
}
