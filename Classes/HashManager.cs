using System;

namespace InvoicesManager.Classes
{
    public class HashManager
    {
        public static string GetMD5HashFromFile(string path)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        internal static string GetMD5HashFromByteArray(byte[] bytes)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
