using InvoicesManager.Classes;
using InvoicesManager.Models;
using InvoicesManager.Windows;
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
        AboutWindow _aboutWindow;
        SettingWindow _settingWindow;
        InvoiceAddWindow _invoiceAddWindow;
        InvoiceEditWindow _invoiceEditWindow;
        InvoiceRemoveWindow _invoiceRemoveWindow;

        List<InvoiceModel> allInvoices = new List<InvoiceModel>();
        private string filterReference = String.Empty;
        private string filterInvoiceNumber = String.Empty;
        private string filterOrganization = "-1";
        private DateTime filterExhibitionDate = default;


        public MainWindow()
        {
            InitializeComponent();
            GenerateAllWindows();
#if DEBUG
            GenerateDebugDataRecords();
#endif
            InitInvoices();
        }

        private void GenerateAllWindows()
        {
            _aboutWindow = new AboutWindow();
            _settingWindow = new SettingWindow();
            _invoiceAddWindow = new InvoiceAddWindow();
            _invoiceEditWindow = new InvoiceEditWindow();
            _invoiceRemoveWindow = new InvoiceRemoveWindow();
        }

        private void GenerateDebugDataRecords()
        {
            Random r = new Random();
            string[] sampleOrganization = { "UPS", "MCDonalds", "Telekom", "DHL", "Amazon", "Apple", "Microsoft", "Google", "Facebook", "Twitter" };

            //Organization debug records
            Comb_Search_Organization.Items.Clear();
            foreach (var organization in sampleOrganization)
                Comb_Search_Organization.Items.Add(organization);

            //Invoices debug records
            Dg_Invoices.Items.Clear();
            for (int i = 0; i < 100; i++)
            {
                InvoiceModel invoice = new InvoiceModel();
                invoice.Reference = "REF" + r.Next(1000, 9999);
                invoice.InvoiceNumber = "INV" + r.Next(1000, 9999);
                invoice.Organization = sampleOrganization[r.Next(0, sampleOrganization.Length)];
                invoice.ExhibitionDate = DateTime.Now.AddDays(r.Next(0, 100));
                invoice.Path = "C:\\Invoices\\" + invoice.Reference + ".pdf";
                
                Dg_Invoices.Items.Add(invoice);
            }
        }

        private void InitInvoices()
        {
            Random r = new Random();
            string[] sampleOrganization = { "UPS", "MCDonalds", "Telekom", "DHL", "Amazon", "Apple", "Microsoft", "Google", "Facebook", "Twitter" };

            //Invoices debug records
            for (int i = 0; i < 100; i++)
            {
                InvoiceModel invoice = new InvoiceModel();
                invoice.Reference = "REF" + r.Next(1000, 9999);
                invoice.InvoiceNumber = "INV" + r.Next(1000, 9999);
                invoice.Organization = sampleOrganization[r.Next(0, sampleOrganization.Length)];
                invoice.ExhibitionDate = DateTime.Now.AddDays(r.Next(0, 100));
                invoice.Path = "C:\\Invoices\\" + invoice.Reference + ".pdf";

                allInvoices.Add(invoice);
            }
        }
        
        private void DG_Invoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            throw new Exception("Not implemented yet");
            object sad = Dg_Invoices.SelectedItem;
        }
        
        private void RefreshDataGrid()
        {
            SortSystem sortSys = new SortSystem(allInvoices, filterReference, filterInvoiceNumber, filterOrganization, filterExhibitionDate);
            List<InvoiceModel> filteredInvoices = sortSys.Sort();

            Dg_Invoices.Items.Clear();
            foreach (var invoice in filteredInvoices)
                Dg_Invoices.Items.Add(invoice);
        }

        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            _invoiceAddWindow.ShowDialog();
        }

        private void Bttn_InvoiceEdit_Click(object sender, RoutedEventArgs e)
        {
            _invoiceEditWindow.ShowDialog();
        }

        private void Bttn_InvoiceRemove_Click(object sender, RoutedEventArgs e)
        {
            _invoiceRemoveWindow.ShowDialog();
        }

        private void Bttn_Settings_Click(object sender, RoutedEventArgs e)
        {
            _settingWindow.ShowDialog();
        }

        private void Bttn_About_Click(object sender, RoutedEventArgs e)
        {
            _aboutWindow.ShowDialog();
        }

        private void Tb_Search_String_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterReference = Tb_Search_String.Text == String.Empty ? String.Empty : Tb_Search_String.Text;
            RefreshDataGrid();
        }

        private void Tb_Search_InvoiceNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterInvoiceNumber = Tb_Search_InvoiceNumber.Text == String.Empty ? String.Empty : Tb_Search_InvoiceNumber.Text;
            RefreshDataGrid();
        }

        private void Comb_Search_Organization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterOrganization = Comb_Search_Organization.SelectedIndex.ToString() == "-1" ? "-1" : Comb_Search_Organization.SelectedItem.ToString();
            RefreshDataGrid();
        }

        private void Dp_Search_ExhibitionDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterExhibitionDate = (DateTime)(Dp_Search_ExhibitionDate.SelectedDate == null ? default(DateTime) : Dp_Search_ExhibitionDate.SelectedDate);
            RefreshDataGrid();
        }
    }
}
