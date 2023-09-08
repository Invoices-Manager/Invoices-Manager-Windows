using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace InvoicesManager.Core
{
    public class EncryptionSystem
    {
        private readonly SecureString userPassword;
        private readonly byte[] salt;

        public EncryptionSystem(SecureString password, string salt)
        {
            userPassword = password;
            this.salt = Encoding.UTF8.GetBytes(salt);
        }

        public string EncryptString(string plainText)
        {
            byte[] encryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();
                aesAlg.GenerateIV();

                byte[] iv = aesAlg.IV;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    encryptedBytes = memoryStream.ToArray();
                }

                byte[] encryptedTextWithIV = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, encryptedTextWithIV, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, encryptedTextWithIV, iv.Length, encryptedBytes.Length);

                return Convert.ToBase64String(encryptedTextWithIV);
            }
        }

        public string DecryptString(string encryptedText)
        {
            byte[] encryptedTextWithIV = Convert.FromBase64String(encryptedText);
            byte[] iv = new byte[16]; // IV-Größe beträgt 16 Bytes für AES-256

            Buffer.BlockCopy(encryptedTextWithIV, 0, iv, 0, iv.Length);
            byte[] encryptedBytes = new byte[encryptedTextWithIV.Length - iv.Length];
            Buffer.BlockCopy(encryptedTextWithIV, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            byte[] decryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();
                aesAlg.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decryptedBytes = Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
                        }
                    }
                }
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public string[] EncryptStringArray(string[] tags)
        {
            string[] encryptedTags = new string[tags.Length];

            for (int i = 0; i < tags.Length; i++)
                encryptedTags[i] = EncryptString(tags[i]);

            return encryptedTags;
        }

        public string[] DecryptStringArray(string[] tags)
        {
            string[] decryptedTags = new string[tags.Length];

            for (int i = 0; i < tags.Length; i++)
                decryptedTags[i] = DecryptString(tags[i]);

            return decryptedTags;
        }

        public void EncryptFile(string inputFile, string outputFile)
        {
            using (FileStream inputStream = File.OpenRead(inputFile))
            using (FileStream outputStream = File.Create(outputFile))
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();
                aesAlg.GenerateIV();

                byte[] iv = aesAlg.IV;

                using (CryptoStream cryptoStream = new CryptoStream(outputStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    cryptoStream.Write(iv, 0, iv.Length);

                    while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, bytesRead);
                    }

                    cryptoStream.FlushFinalBlock();
                }
            }
        }

        public void DecryptFile(string inputFile, string outputFile)
        {
            using (FileStream inputStream = File.OpenRead(inputFile))
            using (FileStream outputStream = File.Create(outputFile))
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();

                byte[] iv = new byte[16]; // IV-Größe beträgt 16 Bytes für AES-256
                inputStream.Read(iv, 0, iv.Length);

                aesAlg.IV = iv;

                using (CryptoStream cryptoStream = new CryptoStream(inputStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        private byte[] GetKey()
        {
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(GetSecureStringBytes(userPassword), salt, 10000))
            {
                return keyDerivationFunction.GetBytes(32); // AES-256 Schlüsselgröße
            }
        }

        private byte[] GetSecureStringBytes(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
                int length = secureString.Length * 2;
                byte[] bytes = new byte[length];
                System.Runtime.InteropServices.Marshal.Copy(unmanagedString, bytes, 0, length);
                return bytes;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
