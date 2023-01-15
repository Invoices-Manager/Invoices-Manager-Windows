using InvoicesManager.Classes;
using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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

                EnvironmentsVariable.AllInvoices.Clear();

                string json = File.ReadAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName);

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    EnvironmentsVariable.AllInvoices = JsonConvert.DeserializeObject<List<InvoiceModel>>(json);

                //set the flag to true
                EnvironmentsVariable.IsInvoiceInitFinish = true;

            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
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
                File.Copy(filePath, newPath);
                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Invoice_System, $"A new invoice has been added. [{newInvoice.FileID}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
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
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Invoice_System, $"A invoice has been edited. [{newInvoice.FileID}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
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
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Invoice_System, $"A invoice has been deleted. [{oldInvoice.FileID}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
            }
        }

        public void SaveAs(InvoiceModel invoice, string path)
        {
            try
            {
                File.Copy(EnvironmentsVariable.PathInvoices + invoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT, path);
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
            }
        }

        public bool CheckIfInvoiceExist(string filePath)
        {
            bool existAlready = false;
            try
            {
                string hashID = HashManager.GetMD5HashFromFile(filePath);
                foreach (var invoice in EnvironmentsVariable.AllInvoices)
                {
                    if (invoice.FileID.Equals(hashID))
                        existAlready = true;
                }

                return existAlready;
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
            }

            return existAlready;
        }

        public bool CheckIfInvoicesDataHasChanged(InvoiceModel backupInvoice)
        {
            bool hasChanged = false;

            var invoice = EnvironmentsVariable.AllInvoices.Find(x => x.FileID.Equals(backupInvoice.FileID));

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
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Invoice_System, ex.Message);
            }
        }


        private void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.AllInvoices, Formatting.Indented));
            
        }
    }
}
