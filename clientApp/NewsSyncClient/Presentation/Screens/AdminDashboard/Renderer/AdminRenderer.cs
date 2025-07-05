using NewsSyncClient.Core.Models.Admin;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Screens;

public class AdminRenderer : IAdminRenderer
{
    public void RenderHeader(string email)
    {
        ConsoleOutputHelper.PrintInfo($"Welcome Admin, {email}");
        ConsoleOutputHelper.PrintInfo($"Date: {DateTime.Now:dd-MMM-yyyy} | Time: {DateTime.Now:hh:mm tt}");
        ConsoleOutputHelper.PrintDivider();
    }

    public void RenderMenu()
    {
        ConsoleOutputHelper.PrintDivider();
        ConsoleOutputHelper.PrintInfo("1. View Server Statuses");
        ConsoleOutputHelper.PrintInfo("2. View Server Details");
        ConsoleOutputHelper.PrintInfo("3. Update Server API Key");
        ConsoleOutputHelper.PrintInfo("4. Add New News Category");
        ConsoleOutputHelper.PrintInfo("5. Logout");
        ConsoleOutputHelper.PrintInfo(""); // for spacing
        ConsoleOutputHelper.PrintInfo("Enter your choice: ", inline: true);
    }

    public Task RenderServerStatusesAsync(List<ServerStatusDto> servers)
    {
        if (servers.Count == 0)
        {
            ConsoleOutputHelper.PrintWarning("No servers found.");
            return Task.CompletedTask;
        }

        ConsoleOutputHelper.PrintHeading("Server Statuses:");
        foreach (var s in servers)
        {
            ConsoleOutputHelper.PrintInfo($"Uptime: {s.Uptime}");
            ConsoleOutputHelper.PrintInfo($"Last Accessed: {s.LastAccessed:dd-MMM-yyyy hh:mm tt}");
            ConsoleOutputHelper.PrintDivider();
        }

        return Task.CompletedTask;
    }

    public Task RenderServerDetailsAsync(List<ServerDetailsDto> details)
    {
        if (details.Count == 0)
        {
            ConsoleOutputHelper.PrintWarning("No server details available.");
            return Task.CompletedTask;
        }

        ConsoleOutputHelper.PrintHeading("Server Details:");
        foreach (var d in details)
        {
            ConsoleOutputHelper.PrintInfo($"ID: {d.Id}");
            ConsoleOutputHelper.PrintInfo($"Server: {d.ServerName}");
            ConsoleOutputHelper.PrintInfo($"API Key: {d.ApiKey}");
            ConsoleOutputHelper.PrintDivider();
        }

        return Task.CompletedTask;
    }
}
