namespace InvoicesManager.Core
{
    public class ConfigSystem
    {
        public void Init()
        {
            LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Config_System, "Config init has been started.");

            try
            {
                string json = String.Empty;

                if (!File.Exists(EnvironmentsVariable.PathConfig + EnvironmentsVariable.ConfigJsonFileName))
                    json = "[]";
                else
                    json = File.ReadAllText(EnvironmentsVariable.PathConfig + EnvironmentsVariable.ConfigJsonFileName);

                ConfigModel config = new ConfigModel()
                {
                    PathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                    UILanguage = "English",
                    ConfigVersion = EnvironmentsVariable.PROGRAM_VERSION,
                    PathLogs = EnvironmentsVariable.PathLogs,
                    MoneyUnit = EnvironmentsVariable.MoneyUnit,
                    CreateABackupEveryTimeTheProgramStarts = EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts,
                    MaxCountBackUp = EnvironmentsVariable.MaxCountBackUp,
                    ColumnVisibility = EnvironmentsVariable.ColumnVisibility
                };

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    config = JsonConvert.DeserializeObject<ConfigModel>(json);

                EnvironmentsVariable.PathPDFBrowser = config.PathPDFBrowser ?? EnvironmentsVariable.PathPDFBrowser;
                EnvironmentsVariable.UILanguage = config.UILanguage ?? EnvironmentsVariable.UILanguage;
                EnvironmentsVariable.ConfigVersion = config.ConfigVersion ?? EnvironmentsVariable.ConfigVersion;
                EnvironmentsVariable.PathLogs = config.PathLogs ?? EnvironmentsVariable.PathLogs;
                EnvironmentsVariable.MoneyUnit = config.MoneyUnit == '\0' ? EnvironmentsVariable.MoneyUnit : config.MoneyUnit;
                EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts = config.CreateABackupEveryTimeTheProgramStarts;
                EnvironmentsVariable.MaxCountBackUp = config.MaxCountBackUp;

                //if the owner starts this program with an older config version, then by default there is NULL, and the program would crash at init
                if (config.ColumnVisibility is not null)
                    EnvironmentsVariable.ColumnVisibility = config.ColumnVisibility;

                SaveIntoJsonFile();
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Config_System, "Config init has been finished.");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Config_System, "Config init has been failed. err: " + ex.Message);
            }
        }

        public void SaveIntoJsonFile()
        {
            ConfigModel config = new ConfigModel()
            {
                PathPDFBrowser = EnvironmentsVariable.PathPDFBrowser,
                UILanguage = EnvironmentsVariable.UILanguage,
                ConfigVersion = EnvironmentsVariable.PROGRAM_VERSION,
                PathLogs = EnvironmentsVariable.PathLogs,
                MoneyUnit = EnvironmentsVariable.MoneyUnit,
                CreateABackupEveryTimeTheProgramStarts = EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts,
                MaxCountBackUp = EnvironmentsVariable.MaxCountBackUp,
                ColumnVisibility = EnvironmentsVariable.ColumnVisibility
            };
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Config_System, "SaveIntoJsonFile() has been called");
#endif
            try
            {
                File.WriteAllText(EnvironmentsVariable.PathConfig + EnvironmentsVariable.ConfigJsonFileName, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Config_System, $"Error saving changes to the config file, err: {ex.Message}");
            }
        }
    }
}