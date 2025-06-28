namespace NewsSync.API.Application.DTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
