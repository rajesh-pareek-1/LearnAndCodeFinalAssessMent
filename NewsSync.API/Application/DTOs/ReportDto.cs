namespace NewsSync.API.Application.DTOs
{
    public class ReportDto
    {
        public int ArticleId { get; set; }
        public string UserId { get; set; } = null!;
        public string? Reason { get; set; }
    }
}