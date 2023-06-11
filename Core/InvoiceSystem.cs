using InvoicesManager.Core.Web;
using System.Buffers.Text;

namespace InvoicesManager.Core
{
    public class InvoiceSystem
    {
        public void Init()
        {
            try
            {
                //set the flag to false
                EnvironmentsVariable.IsInvoiceInitFinish = false;
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, "set EnvironmentsVariable.IsInvoiceInitFinish to false");
#endif
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Invoice_System, "Initializing the invoices...");

                EnvironmentsVariable.AllInvoices.Clear();
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, "Loading the invoices from the json file...");
#endif
                string json = InvoiceWebSystem.GetAll();
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, "Finished, loading the invoices from the json file");
#endif
                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    EnvironmentsVariable.AllInvoices = JsonConvert.DeserializeObject<List<InvoiceModel>>(json);

                //remove the unessary spaces from the tags
                EnvironmentsVariable.AllInvoices.ForEach(x => x.Tags = x.Tags.Select(y => y.Trim()).ToArray());

                //set the flag to true
                EnvironmentsVariable.IsInvoiceInitFinish = true;
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, "set EnvironmentsVariable.IsInvoiceInitFinish to true");
#endif
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while initializing the invoices, err: " + ex.Message);
            }
        }

        public void AddInvoice(InvoiceModel newInvoice, string filePath)
        {
            try
            {
                if (CheckIfInvoiceExist(filePath))
                {
                    MessageBox.Show(Application.Current.Resources["fileDoNotExist"] as string);
                    return;
                }

                //convert the file to base64
                string base64 = Convert.ToBase64String(File.ReadAllBytes(filePath));

                //save into web
                int id = InvoiceWebSystem.Add(newInvoice, base64);
                if (id == -1 || id == 0)
                    throw new Exception("Error while adding a new invoice");
                //save into env
                newInvoice.Id = id;
                EnvironmentsVariable.AllInvoices.Add(newInvoice);
                
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Invoice_System, $"A new invoice has been added. [{newInvoice.FileID}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while adding a new invoice, err: " + ex.Message);
            }
        }

        public void EditInvoice(InvoiceModel oldInvoice, InvoiceModel newInvoice)
        {
            try
            {
                //save into web
                newInvoice.Id = oldInvoice.Id;
                if (InvoiceWebSystem.Edit(newInvoice))
                    throw new Exception("Error while editing a invoice");
                //save into env
                EnvironmentsVariable.AllInvoices.Remove(oldInvoice);
                EnvironmentsVariable.AllInvoices.Add(newInvoice);
                
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Invoice_System, $"A invoice has been edited. [{newInvoice.FileID}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while editing a invoice, err: " + ex.Message);
            }
        }

        public void RemoveInvoice(InvoiceModel oldInvoice)
        {
            try
            {
                //  save into web
                if (InvoiceWebSystem.Remove(oldInvoice.Id))
                    throw new Exception("Error while deleting a invoice");
                //save into env
                EnvironmentsVariable.AllInvoices.Remove(oldInvoice);

                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Invoice_System, $"A invoice has been deleted. [{oldInvoice.FileID}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while deleting a invoice, err: " + ex.Message);
            }
        }

        public void SaveAs(InvoiceModel invoice, string path)
        {
            try
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Invoice_System, $"A invoice has been saved as. ID: [{invoice.FileID}] Path: [{path}]");
                
                string base64 = InvoiceWebSystem.GetFile(invoice.Id);

                File.WriteAllBytes(path, Convert.FromBase64String(base64));
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while saving a invoice as, err: " + ex.Message);
            }
        }

        public bool CheckIfInvoiceExist(string filePath)
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, $"CheckIfInvoiceExist() was called");
#endif
            bool existAlready = false;
            
            try
            {
                string hashID = SecuritySystem.GetMD5HashFromFile(filePath);
                foreach (var invoice in EnvironmentsVariable.AllInvoices)
                {
                    if (invoice.FileID.Equals(hashID))
                        existAlready = true;
                }

                return existAlready;
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while checking if the invoice exist, err: " + ex.Message);
            }

            return existAlready;
        }

        public bool CheckIfInvoicesDataHasChanged(InvoiceModel backupInvoice)
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, $"CheckIfInvoicesDataHasChanged() was called");
#endif
            bool hasChanged = false;

            InvoiceModel invoice;

            try
            {
                invoice = EnvironmentsVariable.AllInvoices.Find(x => x.FileID.Equals(backupInvoice.FileID));
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while getting the invoice, err: " + ex.Message);
                return false;
            }

            if (invoice.Reference != backupInvoice.Reference)
                hasChanged = true;

            if (invoice.DocumentType != backupInvoice.DocumentType)
                hasChanged = true;

            if (invoice.Organization != backupInvoice.Organization)
                hasChanged = true;

            if (invoice.InvoiceNumber != backupInvoice.InvoiceNumber)
                hasChanged = true;

            if (invoice.Tags.Equals(backupInvoice.Tags))
                hasChanged = true;

            if (invoice.ImportanceState != backupInvoice.ImportanceState)
                hasChanged = true;

            if (invoice.MoneyState != backupInvoice.MoneyState)
                hasChanged = true;

            if (invoice.PaidState != backupInvoice.PaidState)
                hasChanged = true;

            if (invoice.MoneyTotal != backupInvoice.MoneyTotal)
                hasChanged = true;

            return hasChanged;
        }

        public void OverrideInvoice(InvoiceModel invoice)
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, $"OverrideInvoice() was called");
#endif
            try
            {
                //find the invoice in the list
                InvoiceModel invoiceToOverride = EnvironmentsVariable.AllInvoices.Find(x => x.FileID == invoice.FileID);

                //edit the invoice (override)
                invoiceToOverride.Reference = invoice.Reference;
                invoiceToOverride.DocumentType = invoice.DocumentType;
                invoiceToOverride.Organization = invoice.Organization;
                invoiceToOverride.InvoiceNumber = invoice.InvoiceNumber;
                invoiceToOverride.Tags = invoice.Tags;
                invoiceToOverride.ImportanceState = invoice.ImportanceState;
                invoiceToOverride.MoneyState = invoice.MoneyState;
                invoiceToOverride.MoneyTotal = invoice.MoneyTotal;
                invoiceToOverride.PaidState = invoice.PaidState;

                //save into web
                if (!InvoiceWebSystem.Edit(invoice))
                    throw new Exception("Error while overriding a invoice");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while overriding a invoice, err: " + ex.Message);
            }
        }
    }
}
