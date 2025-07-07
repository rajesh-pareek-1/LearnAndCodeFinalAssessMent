using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Categories;
using NewsSyncClient.Core.Models.Notifications;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class NotificationServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<ISessionContext> _sessionMock;
        private readonly NotificationService _service;

        public NotificationServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _sessionMock = new Mock<ISessionContext>();
            _service = new NotificationService(_apiClientMock.Object, _sessionMock.Object);
        }

        [Fact]
        public async Task GetNotificationsAsync_ShouldThrow_WhenUserIdIsMissing()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.GetNotificationsAsync();

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to fetch notifications.");
        }

        [Fact]
        public async Task GetNotificationsAsync_ShouldCallApi_WhenUserIdPresent()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");
            var notifications = new List<NotificationDto> { new NotificationDto() };

            _apiClientMock
                .Setup(api => api.GetAsync<List<NotificationDto>>("/api/notification?userId=user123"))
                .ReturnsAsync(notifications);

            var result = await _service.GetNotificationsAsync();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetNotificationCategoriesAsync_ShouldCallApi()
        {
            var categories = new List<CategoryDto> { new CategoryDto() };
            _apiClientMock
                .Setup(api => api.GetAsync<List<CategoryDto>>("/api/categories/article"))
                .ReturnsAsync(categories);

            var result = await _service.GetNotificationCategoriesAsync();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserNotificationPreferencesAsync_ShouldThrow_WhenUserIdIsEmpty()
        {
            Func<Task> act = async () => await _service.GetUserNotificationPreferencesAsync("");

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("User ID cannot be empty.");
        }

        [Fact]
        public async Task GetUserNotificationPreferencesAsync_ShouldCallApi_WhenUserIdValid()
        {
            var prefs = new List<NotificationConfigDto> { new NotificationConfigDto() };

            _apiClientMock
                .Setup(api => api.GetAsync<List<NotificationConfigDto>>("/api/notification/configure?userId=user123"))
                .ReturnsAsync(prefs);

            var result = await _service.GetUserNotificationPreferencesAsync("user123");

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task ConfigureNotificationAsync_ShouldThrow_WhenUserIdMissing()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns<string>(null);

            Func<Task> act = async () => await _service.ConfigureNotificationAsync("Politics", true);

            await act.Should().ThrowAsync<UserInputException>()
                .WithMessage("You must be logged in to configure notifications.");
        }

        [Fact]
        public async Task ConfigureNotificationAsync_ShouldThrow_WhenCategoryNameEmpty()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");

            Func<Task> act = async () => await _service.ConfigureNotificationAsync("", true);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Category name cannot be empty.");
        }

        [Fact]
        public async Task ConfigureNotificationAsync_ShouldCallApi_WhenValidInput()
        {
            _sessionMock.SetupGet(s => s.UserId).Returns("user123");

            _apiClientMock
                .Setup(api => api.PutAsync("/api/notification/configure", It.IsAny<object>()))
                .ReturnsAsync(true);

            var result = await _service.ConfigureNotificationAsync("Politics", true);

            result.Should().BeTrue();
        }
    }
}
