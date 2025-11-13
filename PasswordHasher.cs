using System.Security.Cryptography;
using System.Text;

namespace AuthLibrary
{
    internal static class PasswordHasher
    {
        // Metodo per verificare la password inserita durante il login
        internal static bool DecodePassword(string password, string storedHashBase64, string storedSaltBase64)
        {
            // 1. Converte salt salvato in byte[]
            byte[] saltBytes = Convert.FromBase64String(storedSaltBase64);

            // 2. Deriva hash dalla password inserita con stesso salt e parametri
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                100_000,
                HashAlgorithmName.SHA256,
                32
            );

            // 3. Confronta hash calcolato con hash salvato in tempo costante
            byte[] storedHashBytes = Convert.FromBase64String(storedHashBase64);
            return CryptographicOperations.FixedTimeEquals(hashBytes, storedHashBytes);
        }

        internal static (string Hash, string Salt) EncodePassword(string password)
        {
            // 1. Genera salt sicuro di 32 byte
            byte[] saltBytes = new byte[32];
            RandomNumberGenerator.Fill(saltBytes);
            string saltBase64 = Convert.ToBase64String(saltBytes);


            // 2. Deriva hash PBKDF2 con SHA256, 100.000 iterazioni, lunghezza 32 byte
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
               Encoding.UTF8.GetBytes(password),
               saltBytes,
               100_000,
               HashAlgorithmName.SHA256,
               32
           );
            string hashBase64 = Convert.ToBase64String(hashBytes);
            return (hashBase64, saltBase64);

        }
    }
}