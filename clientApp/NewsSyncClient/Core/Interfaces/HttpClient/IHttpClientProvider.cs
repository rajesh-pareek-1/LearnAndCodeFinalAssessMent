namespace NewsSyncClient.Core.Interfaces;

public interface IHttpClientProvider
{
    HttpClient Client { get; }
    void SetJwtToken(string token);
}
