using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models.Dtos
{
    public class BokningDtos
    {
        public record CreateBookingDto(
            int BordId,
            DateTime StartTime,
            [Range(1, 100)] int AntalGaster,
            int? KundId,
            [Required] string? KundNamn,
            [Required] string? KundTelefon
        );
    }
}
