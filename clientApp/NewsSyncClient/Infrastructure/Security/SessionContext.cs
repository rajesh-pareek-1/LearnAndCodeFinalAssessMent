using NewsSyncClient.Core.Interfaces;

namespace NewsSyncClient.Infrastructure.Security;

public class SessionContext : ISessionContext
{
    public string? JwtToken { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(JwtToken);

    public void Clear()
    {
        JwtToken = null;
        UserId = null;
        Email = null;
        Role = null;
    }
}
