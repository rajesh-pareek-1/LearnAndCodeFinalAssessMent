using NewsSyncClient.Core.Exceptions;
using NewsSyncClient.Core.Interfaces.Renderer;
using NewsSyncClient.Presentation.Helpers;

namespace NewsSyncClient.Presentation.Renderers;

public class ConsoleErrorRenderer : IErrorRenderer
{
    public void Render(Exception ex)
    {
        switch (ex)
        {
            case ApiException api:
                ConsoleOutputHelper.PrintError($"[API ERROR] StatusCode: {api.StatusCode} - {api.Message}");
                break;

            case ValidationException val:
                ConsoleOutputHelper.PrintError($"[VALIDATION ERROR] {val.Message}");
                foreach (var err in val.Errors)
                {
                    ConsoleOutputHelper.PrintError($"• {err.Key}: {string.Join(", ", err.Value)}");
                }
                break;

            case UserInputException input:
                ConsoleOutputHelper.PrintError($"[INPUT ERROR] {input.Message}");
                break;

            default:
                ConsoleOutputHelper.PrintError($"[ERROR] {ex.Message}");
                break;
        }
    }
}
