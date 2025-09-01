using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models
{
    public class Kunder
    {
        public int Id { get; set; }
        [Required, MaxLength(100)] 
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(30)] 
        public string Phone { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Email { get; set; }
        public ICollection<Bokning> Bokningar { get; set; } = new List<Bokning>();

    }
}
