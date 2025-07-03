namespace NewsSyncClient.Core.Interfaces;

public interface ISessionContext
{
    string? JwtToken { get; set; }
    string? UserId { get; set; }
    string? Email { get; set; }
    string? Role { get; set; }

    bool IsAuthenticated { get; }
    void Clear();
}
