namespace NewsSyncClient.Core.Models.Articles;

public class ArticleDto
{
    public int Id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? AuthorName { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public bool IsLiked { get; set; }
    public bool IsDisliked { get; set; }
}
