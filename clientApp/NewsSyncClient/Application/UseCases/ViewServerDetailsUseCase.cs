using NewsSyncClient.Core.Interfaces.Services;
using NewsSyncClient.Core.Interfaces.UseCases;

namespace NewsSyncClient.Application.UseCases;

public class ViewServerDetailsUseCase : IViewServerDetailsUseCase
{
    private readonly IAdminService _service;
    private readonly IAdminRenderer _renderer;

    public ViewServerDetailsUseCase(IAdminService service, IAdminRenderer renderer)
    {
        _service = service;
        _renderer = renderer;
    }

    public async Task ExecuteAsync()
    {
        var details = await _service.GetServerDetailsAsync();
        await _renderer.RenderServerDetailsAsync(details);
    }
}
