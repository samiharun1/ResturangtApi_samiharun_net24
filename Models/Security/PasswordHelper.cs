using System.Security.Cryptography;
using System.Text;

namespace ResturangtApi_samiharun_net24.Models.Security
{
    public class PasswordHelper
    {
        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16); // Slumpmässigt salt
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




