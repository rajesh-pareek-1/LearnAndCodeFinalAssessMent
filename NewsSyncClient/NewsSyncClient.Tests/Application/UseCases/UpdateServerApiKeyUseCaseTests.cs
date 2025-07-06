using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class UpdateServerApiKeyUseCaseTests
    {
        private readonly Mock<IAdminService> _adminServiceMock;
        private readonly UpdateServerApiKeyUseCase _useCase;

        public UpdateServerApiKeyUseCaseTests()
        {
            _adminServiceMock = new Mock<IAdminService>();
            _useCase = new UpdateServerApiKeyUseCase(_adminServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ValidInput_ShouldCallUpdateAndPrintSuccess()
        {
            ConsoleInputHelperTest.SetNextInt(42);
            ConsoleInputHelperTest.SetNextString("new-api-key");
            _adminServiceMock.Setup(x => x.UpdateServerApiKeyAsync(42, "new-api-key")).ReturnsAsync(true);

            await _useCase.ExecuteAsync();

            _adminServiceMock.Verify(x => x.UpdateServerApiKeyAsync(42, "new-api-key"), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ServiceReturnsFalse_ShouldPrintError()
        {
            ConsoleInputHelperTest.SetNextInt(123);
            ConsoleInputHelperTest.SetNextString("fail-key");
            _adminServiceMock.Setup(x => x.UpdateServerApiKeyAsync(123, "fail-key")).ReturnsAsync(false);

            await _useCase.ExecuteAsync();

            _adminServiceMock.Verify(x => x.UpdateServerApiKeyAsync(123, "fail-key"), Times.Once);
        }
    }

    internal static class ConsoleInputHelperTest
    {
        private static int _intToReturn;
        private static string _stringToReturn;

        public static void SetNextInt(int value) => _intToReturn = value;
        public static void SetNextString(string value) => _stringToReturn = value;

        static ConsoleInputHelperTest()
        {
            ConsoleInputHelper.ReadPositiveInt = _ => _intToReturn;
            ConsoleInputHelper.ReadRequiredString = _ => _stringToReturn;
        }
    }
}
