namespace NewsSync.API.Models.Domain
{
    public class ArticleReaction
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public bool IsLiked { get; set; } // true = like, false = dislike

        public DateTime ReactedAt { get; set; } = DateTime.UtcNow;
    }
}
