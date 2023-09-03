using System.Runtime.InteropServices;
using System.Security;

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


        public static string HOST_PROT = "http or https";
        public static string HOST_ADDRESS = "example.com or 192.168.178.45";
        public static string HOST_PORT = "6000 or what you use";
        public const string HOST_PATH = "/api/v01";
        public static string HOST_ENDPOINT { get { return HOST_PROT + "://" + HOST_ADDRESS + ":" + HOST_PORT + HOST_PATH; } }

        public static string API_ENDPOINT_NOTE { get { return HOST_ENDPOINT + "/Note"; } }
        public static string API_ENDPOINT_NOTE_GETALL { get { return API_ENDPOINT_NOTE + "/GetAll"; } }

        public static  string API_ENDPOINT_INVOICE { get { return HOST_ENDPOINT + "/Invoice"; } }
        public static  string API_ENDPOINT_INVOICE_GETALL { get { return API_ENDPOINT_INVOICE + "/GetAll"; } }
        public static string API_ENDPOINT_INVOICE_GETFILE { get { return API_ENDPOINT_INVOICE + "/GetFile"; } }

        public static string API_ENDPOINT_USER { get { return HOST_ENDPOINT + "/User"; } }
        public static string API_ENDPOINT_USER_LOGIN { get { return API_ENDPOINT_USER + "/Login"; } }
        public static string API_ENDPOINT_USER_LOGOUT { get { return API_ENDPOINT_USER + "/Logout"; } }
        public static string API_ENDPOINT_USER_WHOAMI { get { return API_ENDPOINT_USER + "/WhoAmI"; } }

        private static string userSalt { get; set; }
        #region  UserSalt Methods
        
        public static void SetUserSalt(string value)
        {
            userSalt = value;
        }

        public static string GetUserSalt()
        {
            return userSalt;
        }
        #endregion

        private static SecureString userPassword { get; set; }
        #region  UserPassword Methods

        public static void ClearUserPassword()
        {
            userPassword?.Dispose();
            userPassword = null;
        }

        public static void SetUserPassword(string value)
        {
            ClearUserPassword();
            if (value != null)
            {
                userPassword = new SecureString();
                foreach (char c in value)
                {
                    userPassword.AppendChar(c);
                }
                userPassword.MakeReadOnly();
            }
        }

        public static SecureString GetUserPassword()
        {
            return userPassword;
        }
        #endregion

        private static SecureString bearerToken;
        #region  BearerToken Methods
        public static string BearerToken
        {
            get { return GetBearerToken(); }
            set { SetBearerToken(value); }
        }

        public static void ClearBearerToken()
        {
            bearerToken?.Dispose();
            bearerToken = null;
        }

        private static string GetBearerToken()
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(bearerToken);
                return Marshal.PtrToStringUni(valuePtr);
            }
            catch (Exception)
            {
                return String.Empty;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private static void SetBearerToken(string value)
        {
            ClearBearerToken();
            if (value != null)
            {
                bearerToken = new SecureString();
                foreach (char c in value)
                {
                    bearerToken.AppendChar(c);
                }
                bearerToken.MakeReadOnly();
            }
        }

        #endregion


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
