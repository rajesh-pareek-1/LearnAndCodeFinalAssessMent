using AutoMapper;
using FluentAssertions;
using Moq;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Services;
using NewsSync.API.Domain.Entities;
using Xunit;

namespace NewsSync.API.Tests.Services.Admin
{
    public class AdminServiceTests
    {
        private readonly Mock<ICategoryRepository> categoryRepositoryMock = new();
        private readonly Mock<IServerRepository> serverRepositoryMock = new();
        private readonly Mock<IArticleRepository> articleRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();

        private readonly AdminService adminService;

        public AdminServiceTests()
        {
            adminService = new AdminService(
                categoryRepositoryMock.Object,
                serverRepositoryMock.Object,
                articleRepositoryMock.Object,
                mapperMock.Object
            );
        }

        [Fact]
        public async Task AddCategoryAsync_Should_Map_And_Save_Category()
        {
            // Arrange
            var dto = new CategoryCreateRequestDto { Name = "Tech" };
            var category = new Category { Name = "Tech" };

            mapperMock.Setup(m => m.Map<Category>(dto)).Returns(category);

            // Act
            await adminService.AddCategoryAsync(dto);

            // Assert
            categoryRepositoryMock.Verify(r => r.AddAsync(category), Times.Once);
            categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetServerStatusAsync_Should_Call_Repository()
        {
            // Arrange
            var expected = new List<ServerStatusDto>();
            serverRepositoryMock.Setup(r => r.GetServerStatusAsync()).ReturnsAsync(expected);

            // Act
            var result = await adminService.GetServerStatusAsync();

            // Assert
            result.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task GetServerDetailsAsync_Should_Call_Repository()
        {
            // Arrange
            var expected = new List<ServerDetailsDto>();
            serverRepositoryMock.Setup(r => r.GetServerDetailsAsync()).ReturnsAsync(expected);

            // Act
            var result = await adminService.GetServerDetailsAsync();

            // Assert
            result.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task UpdateServerApiKeyAsync_Should_Update_ApiKey()
        {
            // Arrange
            var server = new ServerDetail { Id = 1, ApiKey = "old" };
            serverRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(server);

            // Act
            await adminService.UpdateServerApiKeyAsync(1, "new-key");

            // Assert
            server.ApiKey.Should().Be("new-key");
            serverRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateServerApiKeyAsync_Should_Throw_If_Not_Found()
        {
            // Arrange
            serverRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((ServerDetail?)null);

            // Act
            var act = async () => await adminService.UpdateServerApiKeyAsync(1, "abc");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Server not found");
        }

        [Fact]
        public async Task BlockArticleAsync_Should_Update_Article_Status()
        {
            // Arrange
            var article = new Article { Id = 10, IsBlocked = false };
            articleRepositoryMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(article);

            // Act
            await adminService.BlockArticleAsync(10, true);

            // Assert
            article.IsBlocked.Should().BeTrue();
            articleRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task BlockArticleAsync_Should_Throw_If_Not_Found()
        {
            // Arrange
            articleRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Article?)null);

            // Act
            var act = async () => await adminService.BlockArticleAsync(99, true);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Article not found");
        }
    }
}
