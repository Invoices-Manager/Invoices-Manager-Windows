namespace InvoicesManager.Core
{
    class SecuritySystem
    {
        public static string GetMD5HashFromFile(string path)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(path);

            try
            {
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Security_System, $"Get MD5 Hash from file: {path}");
#endif
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Security_System, "Error while getting MD5 Hash from file, err:" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static string GetMD5HashFromByteArray(byte[] bytes)
        {
            using var md5 = MD5.Create();
            
            try
            {
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Security_System, "Get MD5 Hash from byte array");
#endif 
                return BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Security_System, "Error while getting MD5 Hash from byte array, err:" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
