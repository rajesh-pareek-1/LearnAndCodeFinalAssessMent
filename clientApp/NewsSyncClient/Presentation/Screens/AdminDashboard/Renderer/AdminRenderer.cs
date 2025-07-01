using NewsSyncClient.Core.Models.Admin;

namespace NewsSyncClient.Presentation.Screens;

public class AdminRenderer : IAdminRenderer
{
    public void RenderHeader(string email)
    {
        Console.WriteLine($"Welcome Admin, {email}");
        Console.WriteLine($"Date: {DateTime.Now:dd-MMM-yyyy} | Time: {DateTime.Now:hh:mm tt}\n");
    }

    public void RenderMenu()
    {
        Console.WriteLine("1. View Server Statuses");
        Console.WriteLine("2. View Server Details");
        Console.WriteLine("3. Update Server API Key");
        Console.WriteLine("4. Add New News Category");
        Console.WriteLine("5. Logout");
        Console.Write("\nEnter your choice: ");
    }

    public Task RenderServerStatusesAsync(List<ServerStatusDto> servers)
    {
        if (servers.Count == 0)
        {
            Console.WriteLine("No servers found.");
            return Task.CompletedTask;
        }

        Console.WriteLine("\nServer Statuses:");
        foreach (var s in servers)
        {
            Console.WriteLine($"Uptime: {s.Uptime}");
            Console.WriteLine($"Last Accessed: {s.LastAccessed:dd-MMM-yyyy hh:mm tt}");
            Console.WriteLine(new string('-', 40));
        }

        return Task.CompletedTask;
    }

    public Task RenderServerDetailsAsync(List<ServerDetailsDto> details)
    {
        if (details.Count == 0)
        {
            Console.WriteLine("No server details available.");
            return Task.CompletedTask;
        }

        Console.WriteLine("\nServer Details:");
        foreach (var d in details)
        {
            Console.WriteLine($"ID: {d.Id}");
            Console.WriteLine($"Server: {d.ServerName}");
            Console.WriteLine($"API Key: {d.ApiKey}");
            Console.WriteLine(new string('-', 40));
        }

        return Task.CompletedTask;
    }
}
