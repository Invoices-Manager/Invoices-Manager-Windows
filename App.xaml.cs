using System.Threading;
using System;
using System.Windows;
using InvoicesManager.Core;

namespace InvoicesManager
{
    public partial class App : Application
    {
        private readonly Mutex InvoiceManagerMutex = new Mutex(true, "{GnM-InvoicesManager-GnM-MadeBySchecher_1}");

        public App()
        {
            try
            {
                //check if it is already claimed
                if (!InvoiceManagerMutex.WaitOne(TimeSpan.Zero, true))
                {
                    //the app is already running
                    MessageBox.Show("Another instance of the app is already running.");
                    return;
                }

                //init the app
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Fatal, Classes.Enums.LogPrefixEnum.System_Thread, ex.Message);
            }
            finally
            {
                //release the mutex
                InvoiceManagerMutex.ReleaseMutex();
            }
        }
    }
}
