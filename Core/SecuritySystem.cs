namespace InvoicesManager.Core
{
    class SecuritySystem
    {
        public static string GetMD5HashFromFile(string path)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            using var stream = System.IO.File.OpenRead(path);

            try
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Security_System, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static string GetMD5HashFromByteArray(byte[] bytes)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            
            try
            {
                return BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Security_System, ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
