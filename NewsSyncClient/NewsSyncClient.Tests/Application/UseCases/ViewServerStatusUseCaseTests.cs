using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Admin;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class ViewServerStatusUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_CallsServiceAndRendererWithServerStatuses()
        {
            var serviceMock = new Mock<IAdminService>();
            var rendererMock = new Mock<IAdminRenderer>();
            var sampleStatuses = new List<ServerStatusDto> { new() { Status = "Online" } };

            serviceMock.Setup(s => s.GetServerStatusesAsync()).ReturnsAsync(sampleStatuses);

            var useCase = new ViewServerStatusUseCase(serviceMock.Object, rendererMock.Object);

            await useCase.ExecuteAsync();

            serviceMock.Verify(s => s.GetServerStatusesAsync(), Times.Once);
            rendererMock.Verify(r => r.RenderServerStatusesAsync(sampleStatuses), Times.Once);
        }
    }
}
