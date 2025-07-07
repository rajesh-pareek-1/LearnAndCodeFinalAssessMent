using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Notifications;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class NotificationPreferencesUseCaseTests
    {
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ISessionContext> _sessionContextMock;
        private readonly NotificationPreferencesUseCase _useCase;

        public NotificationPreferencesUseCaseTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _sessionContextMock = new Mock<ISessionContext>();
            _useCase = new NotificationPreferencesUseCase(_notificationServiceMock.Object, _sessionContextMock.Object);
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnsCorrectMappedCategories()
        {
            var categories = new List<CategoryDto>
            {
                new() { Id = 1, Name = "Tech" },
                new() { Id = 2, Name = "Health" }
            };

            var preferences = new List<NotificationConfigDto>
            {
                new() { CategoryId = 1 }
            };

            _sessionContextMock.Setup(s => s.UserId).Returns("user-123");
            _notificationServiceMock.Setup(s => s.GetNotificationCategoriesAsync()).ReturnsAsync(categories);
            _notificationServiceMock.Setup(s => s.GetUserNotificationPreferencesAsync("user-123")).ReturnsAsync(preferences);

            var result = await _useCase.GetCategoriesAsync();

            Assert.Equal(2, result.Count);
            Assert.True(result.First(c => c.Name == "Tech").IsEnabled);
            Assert.False(result.First(c => c.Name == "Health").IsEnabled);
        }

        [Fact]
        public async Task GetCategoriesAsync_ThrowsIfUserIdIsNull()
        {
            _sessionContextMock.Setup(s => s.UserId).Returns((string)null);
            await Assert.ThrowsAsync<UserInputException>(() => _useCase.GetCategoriesAsync());
        }

        [Fact]
        public async Task UpdateCategoryPreferenceAsync_CallsServiceWithCorrectValues()
        {
            var categoryName = "Politics";
            var enabled = true;

            _notificationServiceMock.Setup(s => s.ConfigureNotificationAsync(categoryName, enabled)).ReturnsAsync(true);

            var result = await _useCase.UpdateCategoryPreferenceAsync(categoryName, enabled);

            Assert.True(result);
        }
    }
}
