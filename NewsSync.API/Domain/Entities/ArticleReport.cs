using NewsSync.API.Domain.Entities;

public class ArticleReport
{
    public int Id { get; set; }

    public int ArticleId { get; set; }              // FK to Article
    public Article Article { get; set; } = null!;   // Nav prop

    public string ReportedByUserId { get; set; } = null!;

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
