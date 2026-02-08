using System.Security.Cryptography;
using System.Text;


//Den här klassen används för att skapa säkra lösenord och
// kontrollera att ett lösenord är korrekt.
//
// Syfte:
// 1. HashPassword(password) tar ett vanligt lösenord och skapar:
//    hash: en krypterad version av lösenordet
//    salt: en slumpmässig extra sträng som gör hashen unik
//    Detta gör det mycket svårt för någon att gissa lösenordet.
// 2. Verify(password, hash, salt) → kontrollerar om ett givet 
//    lösenord stämmer med den sparade hash+salt.
//
// Viktiga saker:
// Saltet gör att samma lösenord alltid får olika hash
// PBKDF2 används som en säker krypteringsmetod
// FixedTimeEquals används för att undvika attacker baserade på tid
//
// Den här klassen används t.ex. när du skapar en ny admin
// eller när någon försöker logga in.

namespace ResturangtApi_samiharun_net24.Models.Security
{
    public class PasswordHelper
    {
        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16); // salt
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations: 100_000,
                HashAlgorithmName.SHA256,
                outputLength: 32); // rätt parameter
            return (hash, salt);
        }

        public static bool Verify(string password, byte[] hash, byte[] salt)
        {
            var testHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations: 100_000,
                HashAlgorithmName.SHA256,
                outputLength: 32); // rätt parameter
            return CryptographicOperations.FixedTimeEquals(testHash, hash);
        }
    }
}




