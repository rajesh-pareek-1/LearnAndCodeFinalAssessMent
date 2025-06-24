using System.Net.Http.Json;

namespace NewsSyncClient.Screens
{
    public static class SignupScreen
    {
        public static async Task ShowAsync(HttpClient client)
        {
            Console.Clear();
            Console.WriteLine("=== Sign Up ===\n");

            var email = Prompt("Email: ");
            var password = Prompt("Password: ");

            var registrationData = new
            {
                Username = email,
                Password = password
            };

            try
            {
                var response = await client.PostAsJsonAsync("api/auth/register", registrationData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("\n✅ Signup successful! Please proceed to login.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"\n❌ Signup failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n🚨 An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress Enter to return.");
            Console.ReadLine();
        }

        private static string Prompt(string label)
        {
            Console.Write(label);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
    }
}
