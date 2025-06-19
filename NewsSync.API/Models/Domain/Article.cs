using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsSync.API.Models.Domain
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Headline { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Source { get; set; } = string.Empty;

        [Required]
        public string Url { get; set; } = string.Empty;

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public string PublishedDate { get; set; } = string.Empty;

        public ICollection<SavedArticle> SavedByUsers { get; set; } = new List<SavedArticle>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
