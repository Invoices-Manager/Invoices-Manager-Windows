using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows;

namespace InvoicesManager.Classes
{
    public class BackUpSystem
    {
        public static bool BackUp(string backupFilePath)
        {
            //refresh all invoices
            InvoiceSystem.Init();
            
            bool WasPerformedCorrectly = false;
            BackUpModel backUp = new BackUpModel();
            List<InvoiceModel> allInvoices = EnvironmentsVariable.allInvoices;
            List<InvoiceBackUpModel> invoices = new List<InvoiceBackUpModel>();

            
            //create all InvoiceBackUpModel and add them into the list
            foreach (InvoiceModel invoice in allInvoices)
            {
                InvoiceBackUpModel tmpBackUp = new InvoiceBackUpModel()
                {
                    Base64 = Convert.ToBase64String(File.ReadAllBytes(invoice.Path)),
                    Invoice = new SubInvoiceBackUpModel()
                    {
                        ExhibitionDate = invoice.ExhibitionDate,
                        Organization = invoice.Organization,
                        DocumentType = invoice.DocumentType,
                        InvoiceNumber = invoice.InvoiceNumber,
                        Reference = invoice.Reference
                    }
                };

                invoices.Add(tmpBackUp);
            }

            //set the creation date
            backUp.DateOfCreation = DateTime.Now;

            //set the entity count
            backUp.EntityCount = invoices.Count;

            //link the invoices list to the backUp
            backUp.Invoices = invoices;

            //serialize the backup into the json object
            string json = JsonConvert.SerializeObject(backUp);

            //write the json into the file
            File.WriteAllText(backupFilePath, json);

            //check if the file was created
            if (File.Exists(backupFilePath))
                WasPerformedCorrectly = true;

            
            if (String.IsNullOrEmpty(json))
                WasPerformedCorrectly = false;

            return WasPerformedCorrectly;
        }

        public static bool Restore(string backupFilePath)
        {
            bool WasPerformedCorrectly = true;
            int alreadyExistCounter = 0;
            List<string> allTempFiles = new List<string>();
            BackUpModel backUp = null;

            try
            {
                //get BackUp from the file
                backUp = JsonConvert.DeserializeObject<BackUpModel>(File.ReadAllText(backupFilePath));
            }
            catch
            {
                WasPerformedCorrectly = false;
            }

            //check if the backup is valid
            if (!WasPerformedCorrectly)
                return false;

            //check if the file was read
            if (backUp == null)
                return false;

            //check if the file is empty
            if (backUp.EntityCount == 0)
            {
                MessageBox.Show("Die Sicherung ist leer!", "Fehler", MessageBoxButton.OK);
                return false;
            }

            //check if the file is corrupted
            if (backUp.Invoices == null)
                return false;

            //go through all sub models, 
            //then decode the file from the 64base
            //check if the file exists and if not create it
            //then create the invoice
            //and finally add it to the system

            try
            {
                foreach (InvoiceBackUpModel invoice in backUp.Invoices)
                {
                    byte[] bytes = Convert.FromBase64String(invoice.Base64);
                    String hashCode = HashManager.GetMD5HashFromByteArray(bytes);
                    string path = Path.Combine(Path.GetTempPath(), hashCode + ".pdf");

                    File.WriteAllBytes(path, bytes);

                    string hashID = HashManager.GetMD5HashFromFile(path);
                    string newPath = @$"{EnvironmentsVariable.PathInvoices}{hashID}.pdf";

                    //add the path, so we can delete it later (we know windows doesn't do it for us :))
                    allTempFiles.Add(path);

                    if (InvoiceSystem.CheckIfInvoiceExist(path))
                    {
                        alreadyExistCounter++;
                        continue;
                    }

                    InvoiceModel tmpInvoice = new InvoiceModel()
                    {
                        ExhibitionDate = invoice.Invoice.ExhibitionDate,
                        Organization = invoice.Invoice.Organization,
                        DocumentType = invoice.Invoice.DocumentType,
                        InvoiceNumber = invoice.Invoice.InvoiceNumber,
                        Reference = invoice.Invoice.Reference,
                        Path = newPath
                    };

                    InvoiceSystem.AddInvoice(tmpInvoice, path, newPath);
                }
            }
            catch
            {
                WasPerformedCorrectly = false;
            }
            
            MessageBox.Show($"Es wurden {backUp.EntityCount - alreadyExistCounter} Rechnungen wiederhergestellt. " + Environment.NewLine +
                                          $"{alreadyExistCounter} Rechnungen wurden übersprungen, da sie bereits existieren. " + Environment.NewLine +
                                          $"Von insgesammten {backUp.EntityCount} Rechnungen", 
                                          "Wiederherstellung abgeschlossen", MessageBoxButton.OK);

            return WasPerformedCorrectly;
        }
    }
}
