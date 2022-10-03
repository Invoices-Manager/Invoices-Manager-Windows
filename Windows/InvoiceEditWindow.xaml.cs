using InvoicesManager.Classes;
using InvoicesManager.Models;
using System;
using System.Collections.Generic;
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
    public partial class InvoiceEditWindow : Window
    {
        InvoiceModel invoice;
        public InvoiceEditWindow(InvoiceModel invoice)
        {
            InitializeComponent();
            this.invoice = invoice;
            LoadInvoiceData();
        }

        private void LoadInvoiceData()
        {
            Tb_FilePath.Text = invoice.Path;
            Tb_Organization.Text = invoice.Organization;
            Tb_Reference.Text = invoice.Reference;
            Tb_InvoiceNumber.Text = invoice.InvoiceNumber;
            Tb_DocumentType.Text = invoice.DocumentType;
            Dp_ExhibitionDate.SelectedDate = invoice.ExhibitionDate;
        }

        private void Bttn_InvoiceEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckIfAllIsValide())
            {
                MessageBox.Show("Please Check you data input", "Error", MessageBoxButton.OK);
                return;
            }

            InvoiceModel editInvoice = new InvoiceModel()
            {
                Path = Tb_FilePath.Text,
                Organization = Tb_Organization.Text,
                Reference = Tb_Reference.Text,
                InvoiceNumber = Tb_InvoiceNumber.Text,
                DocumentType = Tb_DocumentType.Text,
                ExhibitionDate = Dp_ExhibitionDate.SelectedDate.Value
            };

            InvoiceSystem.EditInvoice(invoice, editInvoice);

            this.Close();
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

            return true;
        }
    }
}
