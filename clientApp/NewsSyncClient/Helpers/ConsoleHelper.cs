namespace NewsSyncConsoleClient.Helpers;

public static class ConsoleHelper
{
    public static void PressEnterToContinue()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
