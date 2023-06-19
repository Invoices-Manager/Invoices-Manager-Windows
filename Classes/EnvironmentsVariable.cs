namespace InvoicesManager.Classes
{
    public class EnvironmentsVariable
    {
        public static MainWindow MainWindowInstance { get; set; }

        public static ColumnVisibilityModel ColumnVisibility = new ColumnVisibilityModel();
        public static List<InvoiceModel> AllInvoices = new List<InvoiceModel>();
        public static List<InvoiceModel> FilteredInvoices = new List<InvoiceModel>();
        public static List<LogModel> FilteredLogs = new List<LogModel>();
        public static NotebookModel Notebook = new NotebookModel();
        public static List<TemplateModel> AllTemplates = new List<TemplateModel>();
        public static volatile bool IsInvoiceInitFinish = false;
        public static string PathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public static string PathLogs = @$"{Environment.CurrentDirectory}\data\logs\";
        public readonly static string PathConfig = @$"{Environment.CurrentDirectory}\data\";
        public static string PathTemplates = @$"{Environment.CurrentDirectory}\data\";
        public static int MaxCountBackUp = 64;
        public static string ConfigJsonFileName = "Config.json";
        public static string TemplatesJsonFileName = "Templates.json";
        public static string ToDayLogJsonFileName { get { return $"Log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt"; } }

        public static string UILanguage = "English";
        public static string[] PossibleUILanguages = { "English", "German" };
        public const string PROGRAM_VERSION = "2.0.0.0";
        public const string PROGRAM_LICENSE = "License: Open source"; // Placeholder
        public static string ConfigVersion;
        public const string PROGRAM_SUPPORTEDFORMAT = ".pdf";
        public static bool CreateABackupEveryTimeTheProgramStarts = false;
        public static bool Window_Notebook_IsClosed = true;
        public static char MoneyUnit = '€';
        public static char[] PossibleMoneyUnits = { '€', '$', '£', '¥', '₽', '₹' };

        //TODO: use secretString instand of string
        public static string BearerToken = String.Empty;
        public const string HOST_PROT = "http";
        public const string HOST_ADDRESS = "localhost";
        public const string HOST_PORT = "5170";
        public const string HOST_PATH = "/api/v01";
        public const string HOST_ENDPOINT = HOST_PROT + "://" + HOST_ADDRESS + ":" + HOST_PORT + HOST_PATH;

        public const string API_ENDPOINT_NOTE = HOST_ENDPOINT + "/Note";
        public const string API_ENDPOINT_NOTE_GETALL = API_ENDPOINT_NOTE + "/GetAll";
        
        public const string API_ENDPOINT_INVOICE = HOST_ENDPOINT + "/Invoice";
        public const string API_ENDPOINT_INVOICE_GETALL = API_ENDPOINT_INVOICE + "/GetAll";
        public const string API_ENDPOINT_INVOICE_GETFILE = API_ENDPOINT_INVOICE + "/GetFile";

        public const string API_ENDPOINT_USER = HOST_ENDPOINT + "/User";
        public const string API_ENDPOINT_USER_LOGIN = API_ENDPOINT_USER + "/Login";
        public const string API_ENDPOINT_USER_LOGOUT = API_ENDPOINT_USER + "/Logout";


        public static void InitWorkPath()
        {
            //create/check the need folders and files
            Directory.CreateDirectory(PathConfig);
            Directory.CreateDirectory(PathLogs);
            
            if (!File.Exists(PathConfig + ConfigJsonFileName))
                File.WriteAllText(PathConfig + ConfigJsonFileName, "[]");
            if (!File.Exists(PathLogs + ToDayLogJsonFileName))
                File.WriteAllText(PathLogs + ToDayLogJsonFileName, "[]");
            if (!File.Exists(PathTemplates + TemplatesJsonFileName))
                File.WriteAllText(PathTemplates + TemplatesJsonFileName, "[]");
        }
    }
}
