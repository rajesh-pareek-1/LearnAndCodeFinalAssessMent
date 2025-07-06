using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NewsSyncClient.Application.Services;
using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Api;
using NewsSyncClient.Core.Models.Admin;
using Xunit;

namespace NewsSyncClient.Tests.Application.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _adminService = new AdminService(_apiClientMock.Object);
        }

        [Fact]
        public async Task GetServerStatusesAsync_ShouldCallCorrectEndpoint()
        {
            var fakeStatuses = new List<ServerStatusDto> { new ServerStatusDto() };
            _apiClientMock
                .Setup(api => api.GetAsync<List<ServerStatusDto>>("/api/admin/server"))
                .ReturnsAsync(fakeStatuses);

            var result = await _adminService.GetServerStatusesAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetServerDetailsAsync_ShouldCallCorrectEndpoint()
        {
            var fakeDetails = new List<ServerDetailsDto> { new ServerDetailsDto() };
            _apiClientMock
                .Setup(api => api.GetAsync<List<ServerDetailsDto>>("/api/admin/server/details"))
                .ReturnsAsync(fakeDetails);

            var result = await _adminService.GetServerDetailsAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateServerApiKeyAsync_ShouldThrowValidationException_WhenIdIsInvalid()
        {
            Func<Task> act = async () => await _adminService.UpdateServerApiKeyAsync(0, "valid");

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Server ID must be a positive number.");
        }

        [Fact]
        public async Task UpdateServerApiKeyAsync_ShouldThrowValidationException_WhenApiKeyIsEmpty()
        {
            Func<Task> act = async () => await _adminService.UpdateServerApiKeyAsync(5, "");

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("API key cannot be empty.");
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldThrowValidationException_WhenNameIsEmpty()
        {
            Func<Task> act = async () => await _adminService.AddCategoryAsync("", "desc");

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Category name cannot be empty.");
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldCallApiClient_WhenInputsAreValid()
        {
            _apiClientMock
                .Setup(api => api.PostAsync("/api/admin/category", It.IsAny<object>()))
                .ReturnsAsync(true);

            var result = await _adminService.AddCategoryAsync("Sports", "All sports news");

            result.Should().BeTrue();
            _apiClientMock.Verify(api =>
                api.PostAsync("/api/admin/category", It.Is<object>(p =>
                    p.GetType().GetProperty("name")?.GetValue(p)?.ToString() == "Sports" &&
                    p.GetType().GetProperty("description")?.GetValue(p)?.ToString() == "All sports news")),
                Times.Once);
        }
    }
}
