using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;

namespace Infrastructure.PasswordHasher
{
    public class Hasher: IHasher
    {
        private const int SaltSize = 16; // 16 bytes salt (adjust as needed)

        public (string HashedPassword, string Salt) HashPassword(string password)
        {
            // Generate a random salt
            byte[] saltBytes = GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = HashPasswordWithSalt(password, saltBytes);

            return (hashedPassword, Convert.ToBase64String(saltBytes));
        }

        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            // Convert the salt string back to bytes
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Hash the provided password with the retrieved salt
            string hashedPasswordAttempt = HashPasswordWithSalt(password, saltBytes);

            // Compare the computed hash with the stored hashed password
            return hashedPasswordAttempt == hashedPassword;
        }

        private string HashPasswordWithSalt(string password, byte[] saltBytes)
        {
            // Convert the password string to byte array
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Concatenate salt and password bytes
            byte[] saltedPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, saltedPasswordBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, saltBytes.Length, passwordBytes.Length);

            // Create a new instance of SHA256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value of the salted password bytes
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);

                // Convert the hash bytes to string representation (hexadecimal)
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

    }

}

