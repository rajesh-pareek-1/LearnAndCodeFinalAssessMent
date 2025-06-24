namespace NewsSync.API.Models.DTO
{
    public class ReportDto
    {
        public int ArticleId { get; set; }
        public string UserId { get; set; } = null!;
        public string? Reason { get; set; }
    }
}