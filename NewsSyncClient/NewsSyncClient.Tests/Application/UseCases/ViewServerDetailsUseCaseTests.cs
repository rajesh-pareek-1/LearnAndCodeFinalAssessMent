using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using NewsSyncClient.Application.UseCases;
using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Models.Admin;

namespace NewsSyncClient.Tests.Application.UseCases
{
    public class ViewServerDetailsUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_CallsServiceAndRendererWithServerDetails()
        {
            var serviceMock = new Mock<IAdminService>();
            var rendererMock = new Mock<IAdminRenderer>();
            var sampleDetails = new List<ServerDetailsDto> { new() { ServerName = "S1" } };

            serviceMock.Setup(s => s.GetServerDetailsAsync()).ReturnsAsync(sampleDetails);

            var useCase = new ViewServerDetailsUseCase(serviceMock.Object, rendererMock.Object);

            await useCase.ExecuteAsync();

            serviceMock.Verify(s => s.GetServerDetailsAsync(), Times.Once);
            rendererMock.Verify(r => r.RenderServerDetailsAsync(sampleDetails), Times.Once);
        }
    }
}
