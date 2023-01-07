using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.IO;

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
        public static int MaxCountBackUp = 64;
        public static string InvoicesJsonFileName = "Invoices.json";
        public static string ConfigJsonFileName = "Config.json";
        public static string NotebooksJsonFileName = "Notebook.json";
        public static string UILanguage = "English";
        public static string[] UILanguages = { "English", "German" };
        public const string PROGRAM_VERSION = "1.3.0.0";
        public static string ConfigVersion;
        public const string PROGRAM_SUPPORTEDFORMAT = ".pdf";
        //0 = dark mode  | 1 = white mode
        public static int REGSystemUsesLightTheme = 1;
        public static bool CreateABackupEveryTimeTheProgramStarts = true;
        public static bool Window_Invoice_IsClosed = true;
        public static bool Window_Notebook_IsClosed = true;
        public static bool Window_Setting_IsClosed = true;
        public static bool Window_About_IsClosed = true;
        public static bool Window_BackUp_IsClosed = true;

        public static void InitWorkPath()
        {
            //create/check the need folders and files
            Directory.CreateDirectory(EnvironmentsVariable.PathInvoices);
            Directory.CreateDirectory(EnvironmentsVariable.PathBackUps);
            Directory.CreateDirectory(EnvironmentsVariable.PathNotebook);
            if (!File.Exists(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName))
                File.WriteAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName, "[]");
            if (!File.Exists(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName))
                File.WriteAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName, "[]");
            if (!File.Exists(EnvironmentsVariable.ConfigJsonFileName))
                File.WriteAllText(EnvironmentsVariable.ConfigJsonFileName, "[]");
        }
    }
}
