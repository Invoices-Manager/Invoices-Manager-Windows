using System.Threading;

namespace InvoicesManager.Classes
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
