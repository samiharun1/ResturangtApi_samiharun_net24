namespace ResturangtApi_samiharun_net24.Models.Dtos
{
    public class BokningReadDto
    {
        public int Id { get; set; }
        // Bokningens unika ID

        public int BordId { get; set; }
        // Vilket bord bokningen gäller

        public string BordNummer { get; set; } = string.Empty;
        // Bordets nummer (det användaren ser i restaurangen)

        public int KundId { get; set; }
        // ID på kunden som bokat

        public string KundNamn { get; set; } = string.Empty;
        // Kundens namn

        public string KundTelefon { get; set; } = string.Empty;
        // Kundens telefonnummer

        public DateTime StartTime { get; set; }
        // När bokningen börjar

        public int AntalGaster { get; set; }
        // Hur många gäster bokningen gäller
    }
}
