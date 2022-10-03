using System;

namespace InvoicesManager.Classes
{
    public class EnvironmentsVariable
    {
        public static readonly string PathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public static readonly string PathInvoices = @$"{Environment.CurrentDirectory}\data\invoices\";
        public static readonly string PathData = @$"{Environment.CurrentDirectory}\data\";
        public static readonly string InvoicesJsonFileName = "Invoices.json";
    }
}
