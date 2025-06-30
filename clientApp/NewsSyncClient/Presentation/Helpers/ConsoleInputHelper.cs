public static class ConsoleInputHelper
{
    public static (DateTime?, DateTime?) ReadDateRange()
    {
        Console.Write("From (yyyy-mm-dd): ");
        var from = Console.ReadLine();

        Console.Write("To (yyyy-mm-dd): ");
        var to = Console.ReadLine();

        if (DateTime.TryParse(from, out var fromDate) && DateTime.TryParse(to, out var toDate))
            return (fromDate, toDate);

        Console.WriteLine("Invalid dates.");
        return (null, null);
    }

    public static string? ReadOptional(string message)
    {
        Console.Write(message);
        return Console.ReadLine()?.Trim();
    }
}
