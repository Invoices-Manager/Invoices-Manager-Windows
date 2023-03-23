namespace InvoicesManager.Core.Sort
{
    public class LogsSortSystem
    {
        private readonly List<LogModel> allLogs;
        private readonly DateTime filterDate = default;
        private readonly LogStateEnum filterLogState = LogStateEnum.FilterPlaceholder;
        private readonly LogPrefixEnum filterLogPrefix = LogPrefixEnum.FilterPlaceholder;
        private readonly string filterMessage = string.Empty;

        public LogsSortSystem(List<LogModel> allLogs,
                                        DateTime filterDate,
                                        LogStateEnum filterLogState,
                                        LogPrefixEnum filterLogPrefix,
                                        string filterMessage)
        {
            this.allLogs = allLogs;
            this.filterDate = filterDate;
            this.filterLogState = filterLogState;
            this.filterLogPrefix = filterLogPrefix;
            this.filterMessage = filterMessage;
        }

        public void Sort()
        {
            try
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.LogSort_System, $"Start sorting invoices");
                DateTime startTime = DateTime.Now;

                EnvironmentsVariable.FilteredLogs = allLogs
                              .Where(x => x.LogData.DateOfTheEvent == filterDate.Date || filterDate == default)
                              .Where(x => x.LogData.State == LoggerSystem.GetEnumStateAsString(filterLogState) || filterLogState == LogStateEnum.FilterPlaceholder)
                              .Where(x => x.LogData.Prefix == LoggerSystem.GetEnumPrefixAsString(filterLogPrefix) || filterLogPrefix == LogPrefixEnum.FilterPlaceholder)
                              .Where(x => x.LogData.Log.ToLower().Contains(filterMessage.ToLower()) || string.IsNullOrEmpty(filterMessage))
                              .ToList();

                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.LogSort_System, $"Stop sorting logs, took {(DateTime.Now - startTime).TotalMilliseconds} ms");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.LogSort_System, $"Error while sorting logs, err: {ex.Message}");
            }
        }
    }
}
