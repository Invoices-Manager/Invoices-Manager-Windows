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
                string json = File.ReadAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName);
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

        public void AddInvoice(InvoiceModel newInvoice, string filePath, string newPath)
        {
            try
            {
                if (CheckIfInvoiceExist(filePath))
                {
                    MessageBox.Show(Application.Current.Resources["fileDoNotExist"] as string);
                    return;
                }
                
                EnvironmentsVariable.AllInvoices.Add(newInvoice);
#if DEBUG
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, $"start FileCp filePath: {filePath}  newPath: {newPath}");
#endif
                File.Copy(filePath, newPath);
                
                SaveIntoJsonFile();
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
                if (!CheckIfInvoiceExist(EnvironmentsVariable.PathInvoices + oldInvoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT))
                {
                    MessageBox.Show(Application.Current.Resources["fileDoNotExist"] as string);
                    return;
                }

                EnvironmentsVariable.AllInvoices.Remove(oldInvoice);
                EnvironmentsVariable.AllInvoices.Add(newInvoice);

                SaveIntoJsonFile();
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
                if (!CheckIfInvoiceExist(EnvironmentsVariable.PathInvoices + oldInvoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT))
                {
                    MessageBox.Show(Application.Current.Resources["fileDoNotExist"] as string);
                    return;
                }
                EnvironmentsVariable.AllInvoices.Remove(oldInvoice);

                File.Delete(EnvironmentsVariable.PathInvoices + oldInvoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT);

                SaveIntoJsonFile();
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
                File.Copy(EnvironmentsVariable.PathInvoices + invoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT, path);
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

                SaveIntoJsonFile();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, "Error while overriding a invoice, err: " + ex.Message);
            }
        }


        private void SaveIntoJsonFile()
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Invoice_System, $"SaveIntoJsonFile() was called");
#endif
            try
            {
                File.WriteAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.AllInvoices, Formatting.Indented));
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Invoice_System, $"Error while saving the invoices into the json file, err: {ex.Message}");
            }
        }
    }
}
