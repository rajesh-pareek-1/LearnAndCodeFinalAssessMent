using NewsSyncClient.Core.Models.Admin;

public interface IAdminRenderer
{
    void RenderHeader(string email);
    void RenderMenu();
    Task RenderServerStatusesAsync(List<ServerStatusDto> servers);
    Task RenderServerDetailsAsync(List<ServerDetailsDto> details);
}
