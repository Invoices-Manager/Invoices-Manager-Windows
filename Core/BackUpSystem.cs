using InvoicesManager.Classes;
using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace InvoicesManager.Core
{
    public class BackUpSystem
    {
        private static InvoiceMainWindow _invoiceMainWindow;

        
        public static void CheckBackUpCount()
        {
            //get all files in the backup folder
            //the files are named by the date and time and the array is sorted by old to new
            //that means the first file is the oldest and the last file is the newest
            string[] files = Directory.GetFiles(EnvironmentsVariable.PathBackUps);

            //if its zero or negativ => no files to delete
            int hasToDeleteCounter = files.Length - EnvironmentsVariable.MaxCountBackUp;

            if (hasToDeleteCounter > 0)
                //delete the oldest files
                for (int i = 0; i < hasToDeleteCounter; i++)
                    try { File.Delete(files[i]); } catch { }
        }

        public static bool BackUp(string backupFilePath, InvoiceMainWindow mainWindow = null)
        {
            bool WasPerformedCorrectly = false;
            try
            {
                //set the main window for the progress bar
                if (mainWindow != null)
                    _invoiceMainWindow = mainWindow;

                //refresh all invoices
                InvoiceSystem.Init();

                WasPerformedCorrectly = false;
                BackUpModel backUp = new BackUpModel();
                //copy the list without the refs, otherwise happens "Collection was changed; enumeration operation must not be executed."
                List<InvoiceModel> allInvoices = new List<InvoiceModel>(EnvironmentsVariable.AllInvoices);
                List<InvoiceBackUpModel> invoices = new List<InvoiceBackUpModel>();

                //clear the progress bar
                if (mainWindow != null)
                    _invoiceMainWindow.ClearInfoProgressBar();

                //set the progress bar max value 
                //2 => SerializeObject & WriteAllText 
                if (mainWindow != null)
                    _invoiceMainWindow.SetInfoProgressMaxValue(allInvoices.Count + 2);

                //create all InvoiceBackUpModel and add them into the list
                foreach (InvoiceModel invoice in allInvoices)
                {
                    InvoiceBackUpModel tmpBackUp = new InvoiceBackUpModel()
                    {
                        Base64 = Convert.ToBase64String(File.ReadAllBytes(EnvironmentsVariable.PathInvoices + invoice.FileID + EnvironmentsVariable.PROGRAM_SUPPORTEDFORMAT)),
                        Invoice = new InvoiceModel()
                        {
                            FileID = invoice.FileID,
                            CaptureDate = invoice.CaptureDate,
                            ExhibitionDate = invoice.ExhibitionDate,
                            Reference = invoice.Reference,
                            DocumentType = invoice.DocumentType,
                            Organization = invoice.Organization,
                            InvoiceNumber = invoice.InvoiceNumber,
                            Tags = invoice.Tags,
                            ImportanceState = invoice.ImportanceState,
                            MoneyState = invoice.MoneyState,
                            PaidState = invoice.PaidState,
                            MoneyTotal = invoice.MoneyTotal
                        }
                    };

                    invoices.Add(tmpBackUp);
                    if (mainWindow != null)
                        _invoiceMainWindow.SetInfoProgressBarValue(1);
                }

                //set the creation date
                backUp.DateOfCreation = DateTime.Now;

                //set the version of the backup
                backUp.BackUpVersion = EnvironmentsVariable.PROGRAM_VERSION;

                //set the entity count
                backUp.EntityCount = invoices.Count;

                //link the invoices list to the backUp
                backUp.Invoices = invoices;

                //link the notebook to the backUp
                backUp.Notebook = EnvironmentsVariable.Notebook;

                //serialize the backup into the json object
                string json = JsonConvert.SerializeObject(backUp);
                if (mainWindow != null)
                    _invoiceMainWindow.SetInfoProgressBarValue(1);

                //write the json into the file
                File.WriteAllText(backupFilePath, json);
                if (mainWindow != null)
                    _invoiceMainWindow.SetInfoProgressBarValue(1);

                //check if the file was created
                if (File.Exists(backupFilePath))
                    WasPerformedCorrectly = true;


                if (String.IsNullOrEmpty(json))
                    WasPerformedCorrectly = false;

                //clear the progress bar
                if (mainWindow != null)
                    _invoiceMainWindow.ClearInfoProgressBar();
            }
            catch
            {
                return false;
            }

            return WasPerformedCorrectly;
        }

        public static bool Restore(string backupFilePath, InvoiceMainWindow mainWindow = null)
        {
            //set the main window for the progress bar
            if (mainWindow != null)
                _invoiceMainWindow = mainWindow;

            bool WasPerformedCorrectly = true;
            int invoice_AlreadyExistCounter = 0;
            int invoice_WasOverwrittenCounter = 0;
            int note_AlreadyExistCounter = 0;
            int note_WasOverwrittenCounter = 0;
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

            //clear the progress bar
            if (mainWindow != null)
                _invoiceMainWindow.ClearInfoProgressBar();

            //set the progress bar max value 
            if (mainWindow != null)
                _invoiceMainWindow.SetInfoProgressMaxValue(backUp.EntityCount + backUp.Notebook.Notebook.Count);

            //check if the backup is valid
            if (!WasPerformedCorrectly)
                return false;

            //check if the file was read
            if (backUp == null)
                return false;

            //check if the file is empty
            if (backUp.EntityCount == 0 && backUp.Notebook.Notebook.Count == 0)
            {
                MessageBox.Show("The backup is empty!", "Error", MessageBoxButton.OK);
                return false;
            }

            //check if the file is corrupted
            if (backUp.Invoices == null)
                return false;

            try
            {
                //restore the invoices

                //go through all sub models, 
                //then decode the file from the 64base
                //check if the file exists and if not create it
                //then create the invoice
                //and finally add it to the system
                
                foreach (InvoiceBackUpModel backUpPacked in backUp.Invoices)
                {
                    //base64 to byte[] (file / invoice)
                    byte[] bytes = Convert.FromBase64String(backUpPacked.Base64);
                    //this md5 hashcode is the new file name
                    string hashID = HashManager.GetMD5HashFromByteArray(bytes);
                    //tempInvoicePath is the tempory path where the file will be saved and later deleted 
                    string tempInvoicePath = Path.Combine(Path.GetTempPath(), hashID + ".pdf");

                    //create the file in the temp folder (will be deleted later)
                    File.WriteAllBytes(tempInvoicePath, bytes);

                    //create the new path for the invoice
                    string newPath = @$"{EnvironmentsVariable.PathInvoices}{hashID}.pdf";

                    //add the path, so we can delete it later (we know windows doesn't do it for us :))
                    allTempFiles.Add(tempInvoicePath);

                    if (InvoiceSystem.CheckIfInvoiceExist(tempInvoicePath))
                    {
                        //check if the invoices data has changed
                        //=> if yes, then override the invoice count up and continue
                        //=> if no, then do nothing (count up and continue)

                        if (InvoiceSystem.CheckIfInvoicesDataHasChanged(backUpPacked.Invoice))
                        {
                            InvoiceSystem.OverrideInvoice(backUpPacked.Invoice);
                            invoice_WasOverwrittenCounter++;
                            continue;
                        }

                        invoice_AlreadyExistCounter++;
                        continue;
                    }

                    InvoiceSystem.AddInvoice(backUpPacked.Invoice, tempInvoicePath, newPath);
                    if (mainWindow != null)
                        _invoiceMainWindow.SetInfoProgressBarValue(1);
                }
            }
            catch
            {
                WasPerformedCorrectly = false;
            }

            
            try
            {
                //restore the notebook

                //go through all note models, 
                //check if the note exists and if not add it else do nothing
                //check if the note that exist whether note is changed and if yes override it else do nothing
                //and finally add it to the system

                //notebook backup is null when the backup is from a older version (v1.2.3.2 or older)
                if (backUp.Notebook != null)
                    foreach (NoteModel note in backUp.Notebook.Notebook)
                    {
                        if (mainWindow != null)
                            _invoiceMainWindow.SetInfoProgressBarValue(1);
                    
                        if (!NotebookSystem.CheckIfNoteExist(note))
                        {
                            NotebookSystem.AddNote(note);
                            continue;
                        }
                    
                        if (NotebookSystem.CheckIfNoteHasChanged(note))
                        {
                            //override == edit
                            NotebookSystem.EditNote(note);
                            note_WasOverwrittenCounter++;
                            continue;
                        }

                        note_AlreadyExistCounter++;
                    }
            }
            catch
            {
                WasPerformedCorrectly = false;
            }

            //clear the progress bar
            if (mainWindow != null)
                _invoiceMainWindow.ClearInfoProgressBar();

            //delete all temp files
            foreach (string file in allTempFiles)
                try { File.Delete(file); } catch { }

            //must be, if the backup is from a older version (v1.2.3.2 or older)
            int noteCount = 0;

            if (backUp.Notebook != null)
                noteCount = backUp.Notebook.Notebook.Count;


            //show the results
            MessageBox.Show($"{backUp.EntityCount - (invoice_AlreadyExistCounter + invoice_WasOverwrittenCounter)} {Application.Current.Resources["invoicesWereRestored"] as string}" + Environment.NewLine +
                                          $"{invoice_AlreadyExistCounter} {Application.Current.Resources["invoicesWereSkipped"] as string}" + Environment.NewLine +
                                          $"{invoice_WasOverwrittenCounter} {Application.Current.Resources["invoicesWereOverwritten"] as string}" + Environment.NewLine +
                                          $"{Application.Current.Resources["fromTotal"] as string} {backUp.EntityCount} {Application.Current.Resources["invoices"] as string}" + Environment.NewLine + 
                                             Environment.NewLine +
                                          $"{noteCount - (note_AlreadyExistCounter + note_WasOverwrittenCounter)} {Application.Current.Resources["notesWereRestored"] as string}" + Environment.NewLine +
                                          $"{note_AlreadyExistCounter} {Application.Current.Resources["notesWereSkipped"] as string}" + Environment.NewLine +
                                          $"{note_WasOverwrittenCounter} {Application.Current.Resources["notesWereOverwritten"] as string}" + Environment.NewLine +
                                          $"{Application.Current.Resources["fromTotal"] as string} {noteCount} {Application.Current.Resources["notes"] as string}",
                                          Application.Current.Resources["recoveryCompleted"] as string, MessageBoxButton.OK);

            return WasPerformedCorrectly;
        }
    }
}
