namespace NewsSync.API.Models
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Headline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
    }
}