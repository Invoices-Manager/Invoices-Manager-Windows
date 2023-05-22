using System.Threading;
using System;
using System.Windows;
using InvoicesManager.Core;
using InvoicesManager.Classes;

namespace InvoicesManager
{
    public partial class App : Application
    {
        private readonly Mutex InvoiceManagerMutex = new Mutex(true, "{GnM-InvoicesManager-GnM-MadeBySchecher_1}");

        public App()
        {
            try
            {
                //program will freeze if the log folder is not created
                EnvironmentsVariable.InitWorkPath();

#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.System_Thread, "The Mutex will be claimed...");
#endif
                //check if it is already claimed
                if (!InvoiceManagerMutex.WaitOne(TimeSpan.Zero, true))
                {
#if DEBUG
                    LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.System_Thread, "The Mutex was claimed successfully!");
#endif
                    //the app is already running
                    MessageBox.Show("Another instance of the app is already running.");
                    return;
                }

                //init the app
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Fatal, LogPrefixEnum.System_Thread, ex.Message);
            }
            finally
            {
                //release the mutex
                //Its should not work, because the app will release the mutex instantly but it works
                InvoiceManagerMutex.ReleaseMutex();
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.System_Thread, "The Mutex was released successfully!");
#endif
            }
        }
    }
}
