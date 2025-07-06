using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Auth;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class SignupServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly SignupService _service;

        public SignupServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _service = new SignupService(_apiClientMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenDtoIsNull()
        {
            var result = await _service.RegisterAsync(null);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Signup data cannot be null");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenUsernameInvalid(string username)
        {
            var dto = new SignupRequestDto { Username = username, Password = "password123" };

            var result = await _service.RegisterAsync(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Username is required");
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenUsernameNotEmail()
        {
            var dto = new SignupRequestDto { Username = "not-an-email", Password = "password123" };

            var result = await _service.RegisterAsync(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Username must be a valid email");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenPasswordMissing(string password)
        {
            var dto = new SignupRequestDto { Username = "user@example.com", Password = password };

            var result = await _service.RegisterAsync(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Password is required");
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenPasswordTooShort()
        {
            var dto = new SignupRequestDto { Username = "user@example.com", Password = "123" };

            var result = await _service.RegisterAsync(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Password must be at least 6 characters");
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccessTrue_WhenApiReturnsTrue()
        {
            var dto = new SignupRequestDto { Username = "user@example.com", Password = "password123" };

            _apiClientMock.Setup(api => api.PostAsync("api/auth/register", dto)).ReturnsAsync(true);

            var result = await _service.RegisterAsync(dto);

            result.Success.Should().BeTrue();
            result.Message.Should().Contain("Signup successful");
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccessFalse_WhenApiReturnsFalse()
        {
            var dto = new SignupRequestDto { Username = "user@example.com", Password = "password123" };

            _apiClientMock.Setup(api => api.PostAsync("api/auth/register", dto)).ReturnsAsync(false);

            var result = await _service.RegisterAsync(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Signup failed");
        }
    }
}
