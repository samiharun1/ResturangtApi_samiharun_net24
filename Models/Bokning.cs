using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models
{
    public class Bokning
    {
        public int Id { get; set; }

        public int BordId { get; set; }
        public Bord Bord { get; set; } = null!;   

        public int KundId { get; set; }
        public Kunder Kunder { get; set; } = null!; 

        public DateTime StartTime { get; set; }
        [Range(1, 100)]
        public int Guests { get; set; }
        public int AntalGaster { get; internal set; }
        // Varaktighet är alltid 2h
    }
}
