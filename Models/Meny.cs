using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models
{
    public class Meny
    {
        public int Id { get; set; }
        [Required, MaxLength(120)] public string Name { get; set; } = string.Empty;
        [Range(0, 100000)] public decimal Price { get; set; }
        [MaxLength(500)] public string? Description { get; set; }
        public bool IsPopular { get; set; }
        
        public string? BildUrl { get; set; } // bara länken text, inte själva bilden den läggs inte här.
    }
}

