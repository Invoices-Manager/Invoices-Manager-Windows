namespace InvoicesManager.Classes
{
    public class EnvironmentsVariable
    {
        public static ColumnVisibilityModel ColumnVisibility = new ColumnVisibilityModel();
        public static List<InvoiceModel> AllInvoices = new List<InvoiceModel>();
        public static List<InvoiceModel> FilteredInvoices = new List<InvoiceModel>();
        public static NotebookModel Notebook = new NotebookModel();
        public static List<TemplateModel> AllTemplates = new List<TemplateModel>();
        public static volatile bool IsInvoiceInitFinish = false;
        public static string PathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public static string PathInvoices = @$"{Environment.CurrentDirectory}\data\invoices\";
        public static string PathBackUps = @$"{Environment.CurrentDirectory}\data\backups\";
        public static string PathNotebook = @$"{Environment.CurrentDirectory}\data\";
        public readonly static string PathConfig = @$"{Environment.CurrentDirectory}\data\";
        public static string PathLog = @$"{Environment.CurrentDirectory}\data\logs\";
        public static string PathTemplates = @$"{Environment.CurrentDirectory}\data\";
        public static int MaxCountBackUp = 64;
        public static string InvoicesJsonFileName = "Invoices.json";
        public static string ConfigJsonFileName = "Config.json";
        public static string NotebooksJsonFileName = "Notebook.json";
        public static string TemplatesJsonFileName = "Templates.json";
        public static string LogJsonFileName { get { return $"Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt"; } }


        public static string UILanguage = "English";
        public static string[] PossibleUILanguages = { "English", "German" };
        public const string PROGRAM_VERSION = "1.4.3.0";
        public const string PROGRAM_LICENSE = "License: Open source"; // Placeholder
        public static string ConfigVersion;
        public const string PROGRAM_SUPPORTEDFORMAT = ".pdf";
        public static bool CreateABackupEveryTimeTheProgramStarts = true;
        public static bool Window_Notebook_IsClosed = true;
        public static char MoneyUnit = '€';
        public static char[] PossibleMoneyUnits = { '€', '$', '£', '¥', '₽', '₹' };
        
        public static void InitWorkPath()
        {
            //create/check the need folders and files
            Directory.CreateDirectory(PathInvoices);
            Directory.CreateDirectory(PathBackUps);
            Directory.CreateDirectory(PathNotebook);
            Directory.CreateDirectory(PathConfig);
            Directory.CreateDirectory(PathLog);

            if (!File.Exists(PathInvoices + InvoicesJsonFileName))
                File.WriteAllText(PathInvoices + InvoicesJsonFileName, "[]");
            if (!File.Exists(PathNotebook + NotebooksJsonFileName))
                File.WriteAllText(PathNotebook + NotebooksJsonFileName, "[]");
            if (!File.Exists(PathConfig + ConfigJsonFileName))
                File.WriteAllText(PathConfig + ConfigJsonFileName, "[]");
            if (!File.Exists(PathLog + LogJsonFileName))
                File.WriteAllText(PathLog + LogJsonFileName, "[]");
            if (!File.Exists(PathTemplates + TemplatesJsonFileName))
                File.WriteAllText(PathTemplates + TemplatesJsonFileName, "[]");
        }
    }
}
