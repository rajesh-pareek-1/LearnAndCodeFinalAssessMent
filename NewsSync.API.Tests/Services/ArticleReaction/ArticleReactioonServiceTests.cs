using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NewsSync.API.Application.DTOs;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Services;
using NewsSync.API.Domain.Common.Messages;
using NewsSync.API.Domain.Entities;
using Xunit;

namespace NewsSync.API.Tests.Services.ArticleReaction
{
    public class ArticleReactionServiceTests
    {
        private readonly Mock`<IArticleReactionRepository>` reactionRepoMock;
        private readonly Mock<ILogger`<ArticleReactionService>`> loggerMock;
        private readonly ArticleReactionService service;

    public ArticleReactionServiceTests()
        {
            reactionRepoMock = new Mock`<IArticleReactionRepository>`();
            loggerMock = new Mock<ILogger`<ArticleReactionService>`>();
            service = new ArticleReactionService(reactionRepoMock.Object, loggerMock.Object);
        }

    [Fact]
        public async Task SubmitReactionAsync_Should_Call_Repository()
        {
            var dto = new ReactionRequestDto { UserId = "user1", ArticleId = 1, IsLiked = true };

    await service.SubmitReactionAsync(dto);

    reactionRepoMock.Verify(r => r.AddOrUpdateReactionAsync(dto), Times.Once);
        }

    [Fact]
        public async Task SubmitReactionAsync_Should_Log_And_Throw_When_Exception_Occurs()
        {
            var dto = new ReactionRequestDto { UserId = "user1", ArticleId = 99, IsLiked = false };
            reactionRepoMock.Setup(r => r.AddOrUpdateReactionAsync(dto)).ThrowsAsync(new Exception("DB down"));

    var act = async () => await service.SubmitReactionAsync(dto);

    var ex = await Assert.ThrowsAsync`<ApplicationException>`(act);
            ex.Message.Should().Be(ValidationMessages.FailedToSubmitReaction);

    loggerMock.VerifyLog(l => l.LogError(It.IsAny`<Exception>`(),
                It.Is`<string>`(msg => msg.Contains("Failed to submit reaction")),
                dto.UserId, dto.ArticleId));
        }

    [Fact]
        public async Task GetReactionsForUserAsync_Should_Return_Articles()
        {
            var reactions = new List`<ArticleReaction>`
            {
                new() { Article = new Article { Id = 1, Headline = "Test" } },
                new() { Article = new Article { Id = 2, Headline = "More News" } }
            };

    reactionRepoMock.Setup(r => r.GetUserReactionsAsync("user1", true)).ReturnsAsync(reactions);

    var result = await service.GetReactionsForUserAsync("user1", true);

    result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
        }

    [Fact]
        public async Task GetReactionsForUserAsync_Should_Log_And_Throw_On_Exception()
        {
            reactionRepoMock.Setup(r => r.GetUserReactionsAsync("userX", null)).ThrowsAsync(new Exception("Timeout"));

    var act = async () => await service.GetReactionsForUserAsync("userX");

    var ex = await Assert.ThrowsAsync`<ApplicationException>`(act);
            ex.Message.Should().Be(ValidationMessages.FailedToFetchReactions);

    loggerMock.VerifyLog(l => l.LogError(It.IsAny`<Exception>`(),
                It.Is `<string>`(msg => msg.Contains("Failed to fetch reactions")),
                "userX", null));
        }
    }
}
