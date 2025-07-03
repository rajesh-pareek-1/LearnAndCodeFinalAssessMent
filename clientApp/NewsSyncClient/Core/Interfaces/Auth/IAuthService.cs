namespace NewsSyncClient.Core.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
}
