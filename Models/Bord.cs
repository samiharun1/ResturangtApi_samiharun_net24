namespace ResturangtApi_samiharun_net24.Models
{
    public class Bord
    {
        public int Id { get; set; }
        public int BordNummer { get; set; } // unikt nummer som står på bordet
        public int Kapacitet { get; set; }    // hur många får plats
        public ICollection<Bokning> Bokningar { get; set; } = new List<Bokning>();

    }
}
