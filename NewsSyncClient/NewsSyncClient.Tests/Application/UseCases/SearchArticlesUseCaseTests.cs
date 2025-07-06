using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Articles;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class SearchArticlesUseCaseTests
    {
        private readonly Mock<ISearchArticleService> _searchServiceMock;
        private readonly SearchArticlesUseCase _useCase;

        public SearchArticlesUseCaseTests()
        {
            _searchServiceMock = new Mock<ISearchArticleService>();
            _useCase = new SearchArticlesUseCase(_searchServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsArticlesFromService()
        {
            var query = "ai";
            var expected = new List<ArticleDto>
            {
                new() { Id = 1, Title = "AI Revolution" },
                new() { Id = 2, Title = "Future of AI" }
            };

            _searchServiceMock.Setup(s => s.SearchAsync(query)).ReturnsAsync(expected);

            var result = await _useCase.ExecuteAsync(query);

            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected[0].Title, result[0].Title);
            Assert.Equal(expected[1].Id, result[1].Id);
        }
    }
}
