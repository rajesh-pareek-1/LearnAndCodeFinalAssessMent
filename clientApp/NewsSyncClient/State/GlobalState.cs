namespace NewsSyncConsoleClient.State
{
    public static class GlobalAppState
    {
        public static string? JwtToken { get; set; }
        public static string? UserId { get; set; }
        public static string? UserRole { get; set; }
        public static string? Email { get; set; }
    }
}
