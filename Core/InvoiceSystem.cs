using InvoicesManager.Classes;
using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Windows;

namespace InvoicesManager.Core
{
    public class InvoiceSystem
    {
        public static void Init()
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

        public static void AddInvoice(InvoiceModel newInvoice, string filePath,  string newPath)
        {
            if (CheckIfInvoiceExist(filePath))
            {
                MessageBox.Show("Die Datei existiert schon im System!");
                return;
            }
            EnvironmentsVariable.AllInvoices.Add(newInvoice);
            File.Copy(filePath, newPath);

            SaveIntoJsonFile();
        }

        public static void EditInvoice(InvoiceModel oldInvoice, InvoiceModel newInvoice)
        {
            if (!CheckIfInvoiceExist(EnvironmentsVariable.PathInvoices + oldInvoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT))
            {
                MessageBox.Show("Die Datei existiert nicht im System!");
                return;
            }
            
            EnvironmentsVariable.AllInvoices.Remove(oldInvoice);
            EnvironmentsVariable.AllInvoices.Add(newInvoice);

            SaveIntoJsonFile();
        }

        public static void RemoveInvoice(InvoiceModel oldInvoice)
        {
            if (!CheckIfInvoiceExist(EnvironmentsVariable.PathInvoices + oldInvoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT))
            {
                MessageBox.Show("Die Datei existiert nicht im System!");
                return;
            }
            EnvironmentsVariable.AllInvoices.Remove(oldInvoice);

            File.Delete(EnvironmentsVariable.PathInvoices + oldInvoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT);

            SaveIntoJsonFile();
        }

        public static void SaveAs(InvoiceModel invoice, string path)
        {
            File.Copy(EnvironmentsVariable.PathInvoices + invoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT, path);
        }

        public static bool CheckIfInvoiceExist(string filePath)
        {
            string hashID = HashManager.GetMD5HashFromFile(filePath);
            bool existAlready = false;
            
            foreach (var invoice in EnvironmentsVariable.AllInvoices)
            {
                if (invoice.FileID.Equals(hashID))
                    existAlready = true;
            }
            
            return existAlready;
        }

        private static void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.AllInvoices));
        }
    }
}
