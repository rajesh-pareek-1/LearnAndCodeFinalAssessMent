namespace NewsSync.API.Application.DTOs
{
    public class ArticleResponseDto
    {
        public int Id { get; set; }
        public string Headline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsBlocked { get; set; }
    }
}
