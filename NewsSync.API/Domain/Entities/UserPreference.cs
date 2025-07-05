namespace NewsSync.API.Domain.Entities
{
    public class UserPreference
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public int CategoryId { get; set; }
        public int Weight { get; set; } = 1;
        public Category? Category { get; set; }
    }

}