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
    public partial class InvoiceRemoveWindow : Window
    {
        InvoiceModel invoice;
        public InvoiceRemoveWindow(InvoiceModel invoice)
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
            Dp_ExhibitionDate.Text = invoice.ExhibitionDate.ToShortDateString();
        }

        private void Bttn_InvoiceRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show("Are you sure you want to remove this invoice?", "Remove Invoice", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                InvoiceSystem.RemoveInvoice(invoice);

            this.Close();
        }
    }
}
