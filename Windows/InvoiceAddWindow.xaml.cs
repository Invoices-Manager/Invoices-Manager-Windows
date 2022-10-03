using InvoicesManager.Models;
using Newtonsoft.Json;
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
    public partial class InvoiceAddWindow : Window
    {
        public InvoiceAddWindow()
        {
            InitializeComponent();
        }

        List<InvoiceModel> allInvoices = new List<InvoiceModel>();

        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            ReadInvoicesFile();
            AddNewInvoice();
            WriteInvoicesFile();
        }
        private void ReadInvoicesFile()
        {
            throw new NotImplementedException();
        }

        private void AddNewInvoice()
        {
            throw new NotImplementedException();
        }

        private void WriteInvoicesFile()
        {
            throw new NotImplementedException();
        }
    }
}
