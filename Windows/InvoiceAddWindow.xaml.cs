using InvoicesManager.Classes;
using InvoicesManager.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InvoicesManager.Windows
{
    public partial class InvoiceAddWindow : Window
    {
        private string filePath = String.Empty;

        public InvoiceAddWindow()
        {
            InitializeComponent();

            //create/check the need folders and files
            Directory.CreateDirectory(EnvironmentsVariable.PathInvoices);
            Directory.CreateDirectory(EnvironmentsVariable.PathData);
            if (!File.Exists(EnvironmentsVariable.PathData + EnvironmentsVariable.InvoicesJsonFileName))
                    File.WriteAllText(EnvironmentsVariable.PathData + EnvironmentsVariable.InvoicesJsonFileName, "[]");
        }

        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckIfAllIsValide())
            {
                MessageBox.Show("Please Check you data input", "Error", MessageBoxButton.OK);
                return;
            }

            AddNewInvoice();
            ClearAllInputs();
        }
        private void Bttn_InvoiceFileAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
                Tb_FilePath.Text = filePath;
            }
        }

        private void ClearAllInputs()
        {
            Tb_FilePath.Text = String.Empty;
            Tb_Organization.Text = String.Empty;
            Tb_Reference.Text = String.Empty;
            Tb_InvoiceNumber.Text = String.Empty;
            Tb_DocumentType.Text = String.Empty;
            filePath = String.Empty;
        }
        private bool CheckIfAllIsValide()
        {
            if (String.IsNullOrWhiteSpace(Tb_Reference.Text))
                return false;
            if (String.IsNullOrWhiteSpace(Tb_InvoiceNumber.Text))
                return false;
            if (String.IsNullOrWhiteSpace(Tb_Organization.Text))
                return false;
            if (String.IsNullOrWhiteSpace(Tb_DocumentType.Text))
                return false;
            if (Dp_ExhibitionDate.SelectedDate == default)
                return false;
            if (filePath == String.Empty)
                return false;

            return true;
        }
        private void AddNewInvoice()
        {
            string hashID = HashManager.GetMD5HashFromFile(filePath);
            string newPath = @$"{EnvironmentsVariable.PathInvoices}{hashID}.pdf";

            InvoiceModel newInvoice = new InvoiceModel()
            {
                InvoiceNumber = Tb_InvoiceNumber.Text,
                Reference = Tb_Reference.Text,
                Organization = Tb_Organization.Text,
                DocumentType = Tb_DocumentType.Text,
                ExhibitionDate = Dp_ExhibitionDate.SelectedDate.Value,
                Path = newPath
            };

            InvoiceSystem.AddInvoice(newInvoice, filePath, newPath);
        }
    }
}
