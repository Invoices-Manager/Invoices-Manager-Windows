namespace InvoicesManager.Classes
{
    public class EnvironmentsVariable
    {
        public static List<InvoiceModel> AllInvoices = new List<InvoiceModel>();
        public static List<InvoiceModel> FilteredInvoices = new List<InvoiceModel>();
        public static NotebookModel Notebook = new NotebookModel();
        public static volatile bool IsInvoiceInitFinish = false;
        public static string PathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public static string PathInvoices = @$"{Environment.CurrentDirectory}\data\invoices\";
        public static string PathBackUps = @$"{Environment.CurrentDirectory}\data\backups\";
        public static string PathNotebook = @$"{Environment.CurrentDirectory}\data\";
        public readonly static string PathConfig = @$"{Environment.CurrentDirectory}\data\";
        public static string PathLog = @$"{Environment.CurrentDirectory}\data\logs\";
        public static int MaxCountBackUp = 64;
        public static string InvoicesJsonFileName = "Invoices.json";
        public static string ConfigJsonFileName = "Config.json";
        public static string NotebooksJsonFileName = "Notebook.json";
        public static string LogJsonFileName { get { return $"Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt"; } }
        public static string UILanguage = "English";
        public static string[] PossibleUILanguages = { "English", "German" };
        public const string PROGRAM_VERSION = "1.3.2.0";
        public static string ConfigVersion;
        public const string PROGRAM_SUPPORTEDFORMAT = ".pdf";
        public static bool CreateABackupEveryTimeTheProgramStarts = true;
        public static bool Window_Notebook_IsClosed = true;
        public static char MoneyUnit = '€';
        public static char[] PossibleMoneyUnits = { '€', '$', '£', '¥', '₽', '₹' };
        
        public static void InitWorkPath()
        {
            //create/check the need folders and files
            Directory.CreateDirectory(EnvironmentsVariable.PathInvoices);
            Directory.CreateDirectory(EnvironmentsVariable.PathBackUps);
            Directory.CreateDirectory(EnvironmentsVariable.PathNotebook);
            Directory.CreateDirectory(EnvironmentsVariable.PathConfig);
            Directory.CreateDirectory(EnvironmentsVariable.PathLog);

            if (!File.Exists(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName))
                File.WriteAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName, "[]");
            if (!File.Exists(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName))
                File.WriteAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName, "[]");
            if (!File.Exists(EnvironmentsVariable.PathConfig + EnvironmentsVariable.ConfigJsonFileName))
                File.WriteAllText(EnvironmentsVariable.PathConfig + EnvironmentsVariable.ConfigJsonFileName, "[]");
            if (!File.Exists(EnvironmentsVariable.PathLog + EnvironmentsVariable.LogJsonFileName))
                File.WriteAllText(EnvironmentsVariable.PathLog + EnvironmentsVariable.LogJsonFileName, "[]");
        }
    }
}
