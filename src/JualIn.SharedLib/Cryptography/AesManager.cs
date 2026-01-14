using System.Security.Cryptography;

namespace JualIn.SharedLib.Cryptography
{
    public static class AesManager
    {
        public static byte[] GenerateKey(int byteSize = 32)
        {
            if (byteSize != 16 && byteSize != 24 && byteSize != 32)
            {
                throw new ArgumentException("Key size must be 16, 24, or 32 bytes.");
            }

            using var aes = Aes.Create();
            aes.GenerateKey();

            return aes.Key.Take(byteSize).ToArray();
        }

        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using MemoryStream msEncrypt = new();
                using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using StreamWriter swEncrypt = new(csEncrypt);
                    swEncrypt.Write(plainText);
                }

                encrypted = msEncrypt.ToArray();
            }

            return encrypted;
        }

        public static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            string plaintext = string.Empty;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using MemoryStream msDecrypt = new(cipherText);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                plaintext = srDecrypt.ReadToEnd();
            }

            return plaintext;
        }

        public static string EncryptBase64Aes(string plainText, byte[] key, byte[] iv)
        {
            var aes = Encrypt(plainText, key, iv);

            return Convert.ToBase64String(aes);
        }

        public static string DecryptBase64Aes(string base64cipherText, byte[] key, byte[] iv)
        {
            var cipher = Convert.FromBase64String(base64cipherText);

            return Decrypt(cipher, key, iv);
        }
    }
}
