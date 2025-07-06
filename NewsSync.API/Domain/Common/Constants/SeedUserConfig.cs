namespace NewsSync.API.Domain.Common.Constants
{
    public static class SeedUserConfig
    {
        public const string AdminEmail = "admin_rajesh@newssync.com";
        public const string AdminPassword = "Admin_Rajesh@123";

        public static readonly List<(string Email, string Password)> DefaultUsers =
        [
            ("aditya@newssync.com", "User@123"),
            ("shivesh@newssync.com", "User@123"),
            ("akshatbhai@newssync.com", "User@123")
        ];
    }
}
