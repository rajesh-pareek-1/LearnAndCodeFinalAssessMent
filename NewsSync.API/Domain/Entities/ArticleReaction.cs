namespace NewsSync.API.Domain.Entities
{
    public class ArticleReaction
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public bool IsLiked { get; set; }

        public DateTime ReactedAt { get; set; } = DateTime.UtcNow;
    }
}
