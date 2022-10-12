using InvoicesManager.Classes;
using System.Threading;

namespace InvoicesManager.Core
{
    public class WaiterSystem
    {
        public static void WaitUntilInvoiceInitFinish()
        {
            while (!EnvironmentsVariable.isInvoiceInitFinish)
                Thread.Sleep(500);
        }
    }
}