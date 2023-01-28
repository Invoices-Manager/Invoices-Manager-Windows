namespace InvoicesManager.Core
{
    class LoggerSystem
    {
        public static void Log(LogStateEnum state, LogPrefixEnum prefix, string message)
        {
            try
            {
                string logState = GetEnumStateAsString(state);
                string logPrefix = GetEnumPrefixAsString(prefix);
                string logPath = EnvironmentsVariable.PathLog + EnvironmentsVariable.LogJsonFileName;
                DateTime logDate = DateTime.Now;
                //preview => [2021-05-01 12:00:00] [Info] [System_Thread] : This is a test message.
                string logMessage = $"[{logDate.ToString("yyyy-MM-dd HH:mm:ss")}] [{logState}] [{logPrefix}]: {message}";

                //get all logs in a list 
                List<LogModel> allLogs = GetAllLogs();

                //add the new log to the list
                allLogs.Add(new LogModel() { FullLog = logMessage, LogData = new SubLogModel() { DateOfTheEvent = logDate, State = logState, Prefix = logPrefix, Log = message } });

                //save the list to the file (one day log file)
                File.WriteAllText(logPath, JsonConvert.SerializeObject(allLogs, Formatting.Indented));

                //write into a long time log file
                File.AppendAllText(EnvironmentsVariable.PathLog + "Log.txt", logMessage + Environment.NewLine);

                //clear the list
                allLogs.Clear();
            }
            catch (Exception ex)
            {
                Log(LogStateEnum.Error, LogPrefixEnum.Logger_System, ex.Message);
            }
        }

        private static List<LogModel> GetAllLogs()
        {
            return JsonConvert.DeserializeObject<List<LogModel>>(File.ReadAllText(EnvironmentsVariable.PathLog + EnvironmentsVariable.LogJsonFileName));
        }

        private static string GetEnumPrefixAsString(LogPrefixEnum prefix)
        {
            return prefix switch
            {
                LogPrefixEnum.System_Thread => "System-Thread",
                LogPrefixEnum.Config_System => "Config-System",
                LogPrefixEnum.BackUp_System => "BackUp-System",
                LogPrefixEnum.Invoice_System => "Invoice-System",
                LogPrefixEnum.Logger_System => "Logger-System",
                LogPrefixEnum.Notebook_System => "Notebook-System",
                LogPrefixEnum.Sort_System => "Sort-System",
                LogPrefixEnum.Security_System => "Security-System",
                LogPrefixEnum.About_View => "About-View",
                LogPrefixEnum.BackUp_View => "BackUp-View",
                LogPrefixEnum.SaveAs_View => "SaveAs-View",
                LogPrefixEnum.Invoice_View => "Invoice-View",
                LogPrefixEnum.MainWindow_View => "MainWindow-View",
                LogPrefixEnum.Notebook_View => "Notebook-View",
                LogPrefixEnum.Setting_View => "Setting-View",
                _ => "Unknown",
            };
        }

        private static string GetEnumStateAsString(LogStateEnum state)
        {
            return state switch
            {
                LogStateEnum.Debug => "DEBUG",
                LogStateEnum.Info => "INFO",
                LogStateEnum.Warning => "WARNING",
                LogStateEnum.Error => "ERROR",
                LogStateEnum.Fatal => "FATAL",
                _ => "UNKNOWN",
            };
        }
    }
}
