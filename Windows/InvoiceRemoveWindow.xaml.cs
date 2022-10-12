using InvoicesManager.Core;
using InvoicesManager.Models;
using System.Windows;

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
            {
                InvoiceSystem.RemoveInvoice(invoice);
                InvoiceSystem.Init();
            }

            this.Close();
        }
    }
}
