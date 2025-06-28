namespace NewsSync.API.Application.DTOs
{
    public class ServerStatusDto
{
    public TimeSpan Uptime { get; set; }
    public DateTime LastAccessed { get; set; }
}
}