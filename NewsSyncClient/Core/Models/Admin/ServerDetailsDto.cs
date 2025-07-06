namespace NewsSyncClient.Core.Models.Admin;

public class ServerDetailsDto
{
    public int Id { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
