using System;
using System.Threading.Tasks;
using Moq;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Services;
using Xunit;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class AddCategoryUseCaseTests
    {
        private readonly Mock<IAdminService> _adminServiceMock;
        private readonly AddCategoryUseCase _useCase;

        public AddCategoryUseCaseTests()
        {
            _adminServiceMock = new Mock<IAdminService>();
            _useCase = new AddCategoryUseCase(_adminServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldAddCategory_WhenInputIsValid()
        {
            ConsoleInputHelperTest.SetInput("Tech\nAll tech news\n");
            _adminServiceMock.Setup(s => s.AddCategoryAsync("Tech", "All tech news")).ReturnsAsync(true);

            await _useCase.ExecuteAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleFailedAdd_WhenServiceReturnsFalse()
        {
            ConsoleInputHelperTest.SetInput("Health\n\n");
            _adminServiceMock.Setup(s => s.AddCategoryAsync("Health", "")).ReturnsAsync(false);

            await _useCase.ExecuteAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleUserInputException()
        {
            ConsoleInputHelperTest.ThrowOnReadRequired = new UserInputException("Mock input error");

            await _useCase.ExecuteAsync();

            ConsoleInputHelperTest.Reset();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleGeneralException()
        {
            ConsoleInputHelperTest.SetInput("Finance\nInvesting\n");
            _adminServiceMock.Setup(s => s.AddCategoryAsync("Finance", "Investing")).ThrowsAsync(new Exception("Boom"));

            await _useCase.ExecuteAsync();
        }
    }

    static class ConsoleInputHelperTest
    {
        private static Queue<string> _inputQueue = new();
        public static Exception? ThrowOnReadRequired;

        public static void SetInput(params string[] inputs)
        {
            _inputQueue = new Queue<string>(inputs);
            ThrowOnReadRequired = null;
            Console.SetIn(new StringReader(string.Join(Environment.NewLine, inputs)));
        }

        public static string ReadRequiredString(string label)
        {
            if (ThrowOnReadRequired != null)
                throw ThrowOnReadRequired;
            return _inputQueue.Dequeue();
        }

        public static string? ReadOptional(string label)
        {
            return _inputQueue.Dequeue();
        }

        public static void Reset()
        {
            _inputQueue.Clear();
            ThrowOnReadRequired = null;
        }
    }
}
