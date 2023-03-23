using System.Security.Policy;

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
                //Check that at least one filter is on when all are off. Then it skips the sorting, saving a lot of time.
                if (!CheckIfAtLeastOneFilter())
                {
                    LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.LogSort_System, $"No filter is on, skipping sorting");
                    EnvironmentsVariable.FilteredLogs = allLogs;
                    return;
                }

                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.LogSort_System, $"Start sorting invoices");
                DateTime startTime = DateTime.Now;

                EnvironmentsVariable.FilteredLogs = allLogs
                              .Where(x => x.LogData.DateOfTheEvent.Date == filterDate.Date || filterDate == default)
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

        private bool CheckIfAtLeastOneFilter()
        {
            if (filterDate != default)
                return true;
            if (filterLogState != LogStateEnum.FilterPlaceholder)
                return true;
            if (filterLogPrefix != LogPrefixEnum.FilterPlaceholder)
                return true;
            if (!string.IsNullOrEmpty(filterMessage))
                return true;

            return false;
        }
    }
}
