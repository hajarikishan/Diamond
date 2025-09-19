using System.Security.Cryptography;

namespace Diamond.API.Services
{
    public static class PasswordHasher
    {
        // config
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 100_000;

        public static (string saltBase64, string hashBase64) HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(KeySize);

            return (Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool VerifyPassword(string password, string saltBase64, string hashBase64)
        {
            var salt = Convert.FromBase64String(saltBase64);
            var expectedHash = Convert.FromBase64String(hashBase64);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(computed, expectedHash);
        }
    }
}
