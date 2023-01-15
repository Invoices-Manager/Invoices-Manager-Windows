using InvoicesManager.Classes;
using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace InvoicesManager.Core
{
    public class ConfigSystem
    {
        public void Init()
        {
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
                    PathInvoice = EnvironmentsVariable.PathInvoices,
                    PathNotebook = EnvironmentsVariable.PathNotebook,
                    PathBackUp = EnvironmentsVariable.PathBackUps,
                    MoneyUnit = EnvironmentsVariable.MoneyUnit,
                    CreateABackupEveryTimeTheProgramStarts = EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts,
                    MaxCountBackUp = EnvironmentsVariable.MaxCountBackUp
                };

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    config = JsonConvert.DeserializeObject<ConfigModel>(json);

                EnvironmentsVariable.PathPDFBrowser = config.PathPDFBrowser;
                EnvironmentsVariable.UILanguage = config.UILanguage;
                EnvironmentsVariable.ConfigVersion = config.ConfigVersion;
                EnvironmentsVariable.PathInvoices = config.PathInvoice;
                EnvironmentsVariable.PathNotebook = config.PathNotebook;
                EnvironmentsVariable.PathBackUps = config.PathBackUp;
                EnvironmentsVariable.MoneyUnit = config.MoneyUnit;
                EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts = config.CreateABackupEveryTimeTheProgramStarts;
                EnvironmentsVariable.MaxCountBackUp = config.MaxCountBackUp;

                Save();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Config_System, ex.Message);
            }
        }

        public void Save()
        {
            ConfigModel config = new ConfigModel()
            {
                PathPDFBrowser = EnvironmentsVariable.PathPDFBrowser,
                UILanguage = EnvironmentsVariable.UILanguage,
                ConfigVersion = EnvironmentsVariable.PROGRAM_VERSION,
                PathInvoice = EnvironmentsVariable.PathInvoices,
                PathNotebook = EnvironmentsVariable.PathNotebook,
                PathBackUp = EnvironmentsVariable.PathBackUps,
                MoneyUnit = EnvironmentsVariable.MoneyUnit,
                CreateABackupEveryTimeTheProgramStarts = EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts,
                MaxCountBackUp = EnvironmentsVariable.MaxCountBackUp
            };
            
            File.WriteAllText(EnvironmentsVariable.PathConfig + EnvironmentsVariable.ConfigJsonFileName, JsonConvert.SerializeObject(config, Formatting.Indented));
        }
    }
}
