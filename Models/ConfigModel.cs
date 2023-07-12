namespace InvoicesManager.Models
{
    public class ConfigModel
    {
        public string ConfigVersion { get; set; }
        public string PathPDFBrowser { get; set; }
        public string UILanguage { get; set; }
        public string PathLogs { get; set; }
        public char MoneyUnit { get; set; }
        public int MaxCountBackUp { get; set; }
        public bool CreateABackupEveryTimeTheProgramStarts { get; set; }
        public ColumnVisibilityModel ColumnVisibility { get; set; }
        public string HOST_PROT { get; set; }
        public string HOST_ADDRESS { get; set; }
        public string HOST_PORT { get; set; }
    }
}
