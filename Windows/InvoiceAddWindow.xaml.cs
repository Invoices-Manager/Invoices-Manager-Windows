using InvoicesManager.Classes;
using InvoicesManager.Core;
using InvoicesManager.Models;
using Microsoft.Win32;
using System;
using System.Windows;

namespace InvoicesManager.Windows
{
    public partial class InvoiceAddWindow : Window
    {
        private string filePath = String.Empty;

        public InvoiceAddWindow()
        {
            InitializeComponent();
            //The date of the exhibition is now by default today's date
            Dp_ExhibitionDate.SelectedDate = DateTime.Now;
        }

        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckIfAllIsValide())
            {
                MessageBox.Show(Application.Current.Resources["checkYouInput"] as string, Application.Current.Resources["error"] as string, MessageBoxButton.OK);
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
            Dp_ExhibitionDate.SelectedDate = default;
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
            InvoiceSystem.Init();
        }
    }
}
