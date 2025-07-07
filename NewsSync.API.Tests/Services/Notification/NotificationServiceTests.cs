using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NewsSync.API.Application.Services;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Exceptions;

namespace NewsSync.API.Tests.Services.Notification;

public class NotificationServiceTests
{
    private readonly Mock<INotificationRepository> _notificationRepoMock = new();
    private readonly Mock<ILogger<NotificationService>> _loggerMock = new();
    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        _service = new NotificationService(_notificationRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_ReturnsNotifications()
    {
        // Arrange
        var userId = "user123";
        var notifications = new List<Notification> { new() { Title = "Welcome" } };
        _notificationRepoMock.Setup(r => r.GetUserNotificationsAsync(userId)).ReturnsAsync(notifications);

        // Act
        var result = await _service.GetUserNotificationsAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(notifications);
    }

    [Fact]
    public async Task GetUserNotificationsAsync_ThrowsApplicationException_OnFailure()
    {
        var userId = "user123";
        _notificationRepoMock.Setup(r => r.GetUserNotificationsAsync(userId)).ThrowsAsync(new Exception("DB failed"));

        var act = async () => await _service.GetUserNotificationsAsync(userId);

        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("*Failed to fetch notifications*");
    }

    [Fact]
    public async Task GetSettingsAsync_ReturnsConfigurations()
    {
        var userId = "user123";
        var configs = new List<NotificationConfiguration> { new() { CategoryId = 1 } };
        _notificationRepoMock.Setup(r => r.GetNotificationSettingsAsync(userId)).ReturnsAsync(configs);

        var result = await _service.GetSettingsAsync(userId);

        result.Should().BeEquivalentTo(configs);
    }

    [Fact]
    public async Task UpdateSettingAsync_Adds_New_Config_When_Enabled_And_Not_Exists()
    {
        var userId = "user123";
        var categoryName = "Politics";
        var category = new Category { Id = 1, Name = categoryName };

        _notificationRepoMock.Setup(r => r.GetCategoryByNameAsync(categoryName)).ReturnsAsync(category);
        _notificationRepoMock.Setup(r => r.GetNotificationConfigurationAsync(userId, category.Id)).ReturnsAsync((NotificationConfiguration?)null);

        var result = await _service.UpdateSettingAsync(userId, categoryName, enabled: true);

        result.Should().BeTrue();
        _notificationRepoMock.Verify(r => r.AddNotificationConfigurationAsync(It.Is<NotificationConfiguration>(
            config => config.UserId == userId && config.CategoryId == category.Id)), Times.Once);
        _notificationRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateSettingAsync_Removes_Config_When_Disabled_And_Exists()
    {
        var userId = "user123";
        var categoryName = "Politics";
        var category = new Category { Id = 2 };
        var config = new NotificationConfiguration { UserId = userId, CategoryId = 2 };

        _notificationRepoMock.Setup(r => r.GetCategoryByNameAsync(categoryName)).ReturnsAsync(category);
        _notificationRepoMock.Setup(r => r.GetNotificationConfigurationAsync(userId, category.Id)).ReturnsAsync(config);

        var result = await _service.UpdateSettingAsync(userId, categoryName, enabled: false);

        result.Should().BeTrue();
        _notificationRepoMock.Verify(r => r.RemoveNotificationConfigurationAsync(config), Times.Once);
        _notificationRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateSettingAsync_ReturnsFalse_WhenCategoryNotFound()
    {
        _notificationRepoMock.Setup(r => r.GetCategoryByNameAsync("Fake")).ReturnsAsync((Category?)null);

        var result = await _service.UpdateSettingAsync("user123", "Fake", true);

        result.Should().BeFalse();
    }
}
