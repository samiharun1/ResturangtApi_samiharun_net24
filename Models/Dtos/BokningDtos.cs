namespace ResturangtApi_samiharun_net24.Models.Dtos
{
    public class BokningDtos
    {
        public record CreateBookingDto(int BordId, DateTime StartTime, int AntalGaster, int? KundId, string? KundNamn, string? KundTelefon);
    }
}

