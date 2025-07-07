using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Auth;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IHttpClientProvider> _clientProviderMock;
        private readonly Mock<ISessionContext> _sessionMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _clientProviderMock = new Mock<IHttpClientProvider>();
            _sessionMock = new Mock<ISessionContext>();
            _authService = new AuthService(
                _apiClientMock.Object,
                _clientProviderMock.Object,
                _sessionMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "secure123";
            var jwtToken = "token123";

            _apiClientMock
                .Setup(api => api.PostAsync<object, LoginResponseDto>(
                    "/api/auth/login", It.IsAny<object>()))
                .ReturnsAsync(new LoginResponseDto
                {
                    JwtToken = jwtToken,
                    UserId = "user123",
                    Role = "Admin"
                });

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            result.Should().BeTrue();
            _clientProviderMock.Verify(p => p.SetJwtToken(jwtToken), Times.Once);
            _sessionMock.VerifySet(s => s.JwtToken = jwtToken);
            _sessionMock.VerifySet(s => s.UserId = "user123");
            _sessionMock.VerifySet(s => s.Email = email);
            _sessionMock.VerifySet(s => s.Role = "Admin");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task LoginAsync_ShouldThrowValidationException_WhenEmailIsInvalid(string email)
        {
            // Act
            Func<Task> act = async () => await _authService.LoginAsync(email, "password");

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Email cannot be empty.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task LoginAsync_ShouldThrowValidationException_WhenPasswordIsInvalid(string password)
        {
            // Act
            Func<Task> act = async () => await _authService.LoginAsync("test@example.com", password);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Password cannot be empty.");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFalse_WhenApiThrowsException()
        {
            // Arrange
            _apiClientMock
                .Setup(api => api.PostAsync<object, LoginResponseDto>(
                    "/api/auth/login", It.IsAny<object>()))
                .ThrowsAsync(new Exception("Network error"));

            // Act
            var result = await _authService.LoginAsync("test@example.com", "password");

            // Assert
            result.Should().BeFalse();
            _clientProviderMock.Verify(p => p.SetJwtToken(It.IsAny<string>()), Times.Never);
            _sessionMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFalse_WhenJwtTokenIsMissing()
        {
            // Arrange
            _apiClientMock
                .Setup(api => api.PostAsync<object, LoginResponseDto>(
                    "/api/auth/login", It.IsAny<object>()))
                .ReturnsAsync(new LoginResponseDto
                {
                    JwtToken = null
                });

            // Act
            var result = await _authService.LoginAsync("test@example.com", "password");

            // Assert
            result.Should().BeFalse();
            _clientProviderMock.Verify(p => p.SetJwtToken(It.IsAny<string>()), Times.Never);
            _sessionMock.VerifyNoOtherCalls();
        }
    }
}
