namespace NewsSyncClient.Core.Interfaces.Api;

public interface IHttpClientProvider
{
    HttpClient Client { get; }
    void SetJwtToken(string token);
}
