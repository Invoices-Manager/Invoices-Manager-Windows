namespace InvoicesManager.Core
{
    public class WaiterSystem
    {
        public static void WaitUntilInvoiceInitFinish()
        {
            while (!EnvironmentsVariable.IsInvoiceInitFinish)
                Thread.Sleep(500);
        }
    }
}