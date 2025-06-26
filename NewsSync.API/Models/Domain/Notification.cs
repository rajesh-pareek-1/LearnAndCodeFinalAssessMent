using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsSync.API.Models.Domain
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Article))]
        public int ArticleId { get; set; }

        public Article Article { get; set; } = null!;

        [Required]
        public DateTime SentAt { get; set; }
    }
}
