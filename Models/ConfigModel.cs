namespace InvoicesManager.Models
{
    public class ConfigModel
    {
        public string ConfigVersion { get; set; }
        public string PathPDFBrowser { get; set; }
        public string UILanguage { get; set; }
        public string PathInvoice { get; set; }
        public string PathNotebook { get; set; }
        public string PathBackUp { get; set; }
        public string PathLogs { get; set; }
        public char MoneyUnit { get; set; }
        public int MaxCountBackUp { get; set; }
        public bool CreateABackupEveryTimeTheProgramStarts { get; set; }
        public ColumnVisibilityModel ColumnVisibility { get; set; }
    }
}
