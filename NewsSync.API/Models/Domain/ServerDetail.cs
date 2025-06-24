using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Models.Domain
{
    public class ServerDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ServerName { get; set; } = string.Empty;

        public DateTime LastAccess { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Unknown";

        [Required]
        public string ApiKey { get; set; } = string.Empty;

        [Required]
        public string BaseUrl { get; set; } = string.Empty;
    }
}
