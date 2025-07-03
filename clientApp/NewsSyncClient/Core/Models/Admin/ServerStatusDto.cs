namespace NewsSyncClient.Core.Models.Admin;

public class ServerStatusDto
{
    public TimeSpan Uptime { get; set; }
    public DateTime LastAccessed { get; set; }
}
