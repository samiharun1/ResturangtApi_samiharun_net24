using System.ComponentModel.DataAnnotations;


// Den här filen innehåller DTO-klasser som används för att
// skicka data mellan frontend (React/MVC) och backend (API).
//
// Syftet med DTOs:
// Bestämma exakt vilka fält som ska skickas fram och tillbaka
// Separera databasen från vad klienten ser
// Göra API-anrop enklare och säkrare
//
// Exempel: BokningReadDto används när vi hämtar bokningar,
// medan CreateBookingDto används när vi skapar en ny bokning.

namespace ResturangtApi_samiharun_net24.Models.Dtos
{

    public class BokningDtos
    {
        public record CreateBookingDto(
          int BordId, // Vilket bord ska bokas
            DateTime StartTime, // När bokningen börjar
            [Range(1, 100)] int AntalGaster, // Antal gäster, min 1 max 100
            int? KundId, // Om vi redan har kund i databasen
            string? KundNamn, // Om det är en ny kund
            string? KundTelefon // Om det är en ny kund
        );
    }
}
