namespace NewsSyncClient.Presentation.Helpers
{
    public static class ConsoleInputHelper
    {
        public static string ReadRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input)) return input;
                Console.WriteLine("Input cannot be empty. Please try again.");
            }
        }

        public static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim();
                if (int.TryParse(input, out var value) && value > 0)
                    return value;
                Console.WriteLine("Please enter a positive integer.");
            }
        }

        public static (DateTime from, DateTime to) ReadDateRange()
        {
            while (true)
            {
                Console.Write("From (yyyy-mm-dd): ");
                var fromStr = Console.ReadLine()?.Trim();
                Console.Write("To   (yyyy-mm-dd): ");
                var toStr = Console.ReadLine()?.Trim();

                if (DateTime.TryParse(fromStr, out var from) && DateTime.TryParse(toStr, out var to))
                    return (from, to);

                Console.WriteLine("Invalid date format. Please try again.");
            }
        }

        public static string? ReadOptional(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim();
        }

        public static bool Confirm(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (y/n): ");
                var input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y") return true;
                if (input == "n") return false;
                Console.WriteLine("Please enter 'y' or 'n'.");
            }
        }

        public static void WaitForUser(string message = "Press Enter to continue...")
        {
            ConsoleOutputHelper.PrintInfo(message, inline: true);
            Console.ReadLine();
        }

        public static string ReadPasswordMasked(string prompt)
        {
            Console.Write(prompt);
            var password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }
}
