namespace ResturangtApi_samiharun_net24.Models
{
    public class Bord
    {
        public int Id { get; set; }
        // Unikt nummer för varje bord i databasen

        public int BordNummer { get; set; }
        // Bordets nummer som man ser i restaurangen (t.ex bord 1, bord 2)

        public int Kapacitet { get; set; }
        // Hur många personer som får plats vid bordet

        public ICollection<Bokning> Bokningar { get; set; } = new List<Bokning>();
        // Alla bokningar som hör till detta bord.
        // ICollection är en lista som Entity Framework kan använda för relationer

    }
}
