namespace BooksApi.Services
{
    using System;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly byte[] salt;

        public PasswordHasherService()
        {
#warning Salt reviwed
            // generate a 128-bit salt using a secure PRNG
            this.salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(this.salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(this.salt)}");
        }

        public string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: this.salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public bool VerifyPassword(string password, string currentHashedPassword)
        {
            if (currentHashedPassword == null)
            {
                throw new ArgumentNullException(nameof(currentHashedPassword));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: this.salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedPassword == currentHashedPassword;
        }
    }
}