using System.Security.Cryptography;
using System.Text;

namespace RentACar.Core.Utilities.Security.Hashing
{
    /// <summary>
    /// Kullanıcı şifrelerinin güvenliği için Hash (Kriptografik Özet) ve Salt (Tuzlama) işlemlerini yürüten yardımcı sınıftır.
    /// </summary>
    public static class HashingHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return CryptographicOperations.FixedTimeEquals(computedHash, passwordHash);
            }
        }
    }
}
