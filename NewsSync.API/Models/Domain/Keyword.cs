using System.ComponentModel.DataAnnotations;

namespace NewsSync.API.Models.Domain
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty; 
    }
}
