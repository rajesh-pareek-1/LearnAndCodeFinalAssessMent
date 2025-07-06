using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Notifications;
using Xunit;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class FetchNotificationsUseCaseTests
    {
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly FetchNotificationsUseCase _useCase;

        public FetchNotificationsUseCaseTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _useCase = new FetchNotificationsUseCase(_notificationServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNotifications()
        {
            var expected = new List<NotificationDto>
            {
                new() { Title = "Update", Message = "App updated" }
            };

            _notificationServiceMock.Setup(x => x.GetNotificationsAsync()).ReturnsAsync(expected);

            var result = await _useCase.ExecuteAsync();

            Assert.Equal(expected, result);
        }
    }
}
