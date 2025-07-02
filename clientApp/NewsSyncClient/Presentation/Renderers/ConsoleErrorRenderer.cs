using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Renderer;

namespace NewsSyncClient.Presentation.Renderers;

public class ConsoleErrorRenderer : IErrorRenderer
{
    public void Render(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        switch (ex)
        {
            case ApiException api:
                Console.WriteLine($"[API ERROR] StatusCode: {api.StatusCode} - {api.Message}");
                break;
            case ValidationException val:
                Console.WriteLine($"[VALIDATION ERROR] {val.Message}");
                foreach (var err in val.Errors)
                {
                    Console.WriteLine($"• {err.Key}: {string.Join(", ", err.Value)}");
                }
                break;
            case UserInputException input:
                Console.WriteLine($"[INPUT ERROR] {input.Message}");
                break;
            default:
                Console.WriteLine($"[ERROR] {ex.Message}");
                break;
        }

        Console.ResetColor();
    }
}
