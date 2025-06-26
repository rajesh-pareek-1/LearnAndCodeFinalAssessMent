namespace NewsSync.API.Models.DTO
{
    public class ServerStatusDto
{
    public TimeSpan Uptime { get; set; }
    public DateTime LastAccessed { get; set; }
}
}