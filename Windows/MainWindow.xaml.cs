using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoicesManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            GenerateDebugDataRecords();
#endif
        }

        private void GenerateDebugDataRecords()
        {
            Random r = new Random();
            string[] sampleOrganization = { "UPS", "MCDonalds", "Telekom", "DHL", "Amazon", "Apple", "Microsoft", "Google", "Facebook", "Twitter" };

            for (int i = 0; i < 100; i++)
            {
                InvoiceModel invoice = new InvoiceModel();
                invoice.Reference = "REF" + r.Next(1000, 9999);
                invoice.InvoiceNumber = "INV" + r.Next(1000, 9999);
                invoice.Organization = sampleOrganization[r.Next(0, sampleOrganization.Length)];
                invoice.ExhibitionDate = DateTime.Now.AddDays(r.Next(0, 100));
                invoice.Path = "C:\\Invoices\\" + invoice.Reference + ".pdf";
                
                DG_Invoices.Items.Add(invoice);
            }
        }


        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_InvoiceEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_InvoiceRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_About_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void DG_Invoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object sad = DG_Invoices.SelectedItem;
        }
    }
}
