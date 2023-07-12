using System.Security;
using System.Text;

namespace InvoicesManager.Core
{
    public class EncryptionSystem
    {
        private readonly SecureString userPassword;
        private readonly byte[] salt;

        public EncryptionSystem(SecureString password, byte[] salt)
        {
            userPassword = password;
            this.salt = salt;
        }

        public string EncryptString(string plainText)
        {
            byte[] encryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();
                aesAlg.IV = aesAlg.Key;

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
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public string DecryptString(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();
                aesAlg.IV = aesAlg.Key;

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

        public void EncryptFile(string inputFile, string outputFile)
        {
            using (FileStream inputStream = File.OpenRead(inputFile))
            using (FileStream outputStream = File.Create(outputFile))
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKey();
                aesAlg.IV = aesAlg.Key;

                using (CryptoStream cryptoStream = new CryptoStream(outputStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

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
                aesAlg.IV = aesAlg.Key;

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
                return keyDerivationFunction.GetBytes(32); // AES-256 key size
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