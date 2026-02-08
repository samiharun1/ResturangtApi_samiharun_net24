using System.ComponentModel.DataAnnotations;

namespace ResturangtApi_samiharun_net24.Models
{
    public class Admin
    {
        public int Id { get; set; }
        // Id är ett unikt nummer som varje admin får automatiskt.
        // Tänk på det som adminens personnummer i databasen.

        [Required, MaxLength(60)]
        public string Username { get; set; } = string.Empty;
        // Username är adminens namn för inloggning.
        // Required = måste fyllas i.
        // MaxLength(60) = får max vara 60 tecken.
        // = string.Empty gör att det inte blir null från början, utan en tom text.

        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        // PasswordHash är lösenordet, men krypterat.
        // Vi sparar aldrig själva lösenordet som text, utan som en hash (en kodad version).

        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        // PasswordSalt är en extra säkerhet som används tillsammans med hash.
        // Det gör det mycket svårare för någon att lista ut lösenordet.
    }

}
