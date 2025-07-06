using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Application.UseCases;

public class ViewServerStatusUseCase : IViewServerStatusUseCase
{
    private readonly IAdminService _service;
    private readonly IAdminRenderer _renderer;

    public ViewServerStatusUseCase(IAdminService service, IAdminRenderer renderer)
    {
        _service = service;
        _renderer = renderer;
    }

    public async Task ExecuteAsync()
    {
        var statuses = await _service.GetServerStatusesAsync();
        await _renderer.RenderServerStatusesAsync(statuses);
    }
}
