namespace InvoicesManager.Models
{
    public class ConfigModel
    {
        public string ConfigVersion { get; set; }
        public string PathPDFBrowser { get; set; }
        public string UILanguage { get; set; }
        public string PathInvoice { get; set; }
        public string PathBackUp { get; set; }
        public bool CreateABackupEveryTimeTheProgramStarts { get; set; }
    }
}
