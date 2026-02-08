using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models
{
    public class Kunder
    {
        public int Id { get; set; }
        // Unikt nummer för varje kund i databasen

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        // Kundens namn, måste finnas, max 100 tecken

        [Required, MaxLength(30)]
        public string Phone { get; set; } = string.Empty;
        // Telefonnummer, måste finnas, max 30 tecken

        [MaxLength(100)]
        public string? Email { get; set; }
        // Kundens email, valfritt fält

        public ICollection<Bokning> Bokningar { get; set; } = new List<Bokning>();
        // Alla bokningar som kunden har gjort

    }
}
