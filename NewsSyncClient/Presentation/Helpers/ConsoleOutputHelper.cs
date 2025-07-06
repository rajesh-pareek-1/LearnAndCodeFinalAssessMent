namespace NewsSyncClient.Presentation.Helpers;

public static class ConsoleOutputHelper
{
    public static void PrintHeader(string title)
    {
        Console.WriteLine($"\n=== {title} ===\n");
    }

    public static void PrintInfo(string message, bool inline = false)
    {
        if (inline)
            Console.Write(message);
        else
            Console.WriteLine(message);
    }

    public static void PrintSuccess(string message, bool inline = false)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        if (inline)
            Console.Write(message);
        else
            Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintError(string message, bool inline = false)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        if (inline)
            Console.Write(message);
        else
            Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintWarning(string message, bool inline = false)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (inline)
            Console.Write(message);
        else
            Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintHeading(string title)
    {
        Console.WriteLine($"\n=== {title} ===\n");
    }

    public static void PrintDivider()
    {
        Console.WriteLine(new string('-', 50));
    }

    public static void PrintDebug(string message, bool inline = false)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        if (inline)
            Console.Write(message);
        else
            Console.WriteLine(message);
        Console.ResetColor();
    }
}
