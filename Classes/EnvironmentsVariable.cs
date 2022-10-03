using System;

namespace InvoicesManager.Classes
{
    public class EnvironmentsVariable
    {
        public static readonly string PathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public static readonly string PathWorkDir = @$"C:\Users\{Environment.UserName}\Documents\IM";
    }
}
