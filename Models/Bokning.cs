using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models
{
    public class Bokning
    {
        public int Id { get; set; }
        // Unikt nummer för varje bokning

        public int BordId { get; set; }
        // Koppling till vilket bord som bokningen gäller
        public Bord Bord { get; set; } = null!;
        // Navigation till bordet, används av EF Core för att hämta bordets info

        public int KundId { get; set; }
        // Koppling till vilken kund som bokat
        public Kunder Kunder { get; set; } = null!;
        // Navigation till kunden

        public DateTime StartTime { get; set; }
        // När bokningen börjar

        [Range(1, 100)]
        public int AntalGaster { get; internal set; }
        // Hur många gäster bokningen gäller (min 1, max 100)
        // Varaktighet är alltid 2 timmar hanteras i API-logiken



        //Jag har relationer mellan tabellerna.
        //En bokning är kopplad till både ett bord och en kund via foreign keys.”
    }
}
