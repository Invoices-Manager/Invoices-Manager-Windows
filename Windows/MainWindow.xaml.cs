using InvoicesManager.Classes;
using InvoicesManager.Models;
using InvoicesManager.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

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

        private const string pathPDFBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

        public MainWindow()
        {
            InitializeComponent();
            GenerateAllWindows();
#if DEBUG
          //  GenerateDebugDataRecords();
#endif
            InitThreads();
        }

        private void InitThreads()
        {
            Thread _initInvoicesThread = new Thread(ThreadTaskGenerateDebugDataRecords);
            Thread _initOrganizationsThread = new Thread(ThreadTaskInitOrganization);
            Thread _refreshDataGridThread = new Thread(ThreadTaskRefreshDataGrid);

            _initInvoicesThread.Start();
            _initInvoicesThread.Join();

            _initOrganizationsThread.Start();
            _initOrganizationsThread.Join();

            _refreshDataGridThread.Start();
            _refreshDataGridThread.Join();
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
            Thread _initInvoicesThread = new Thread(ThreadTaskGenerateDebugDataRecords);
            _initInvoicesThread.Priority = ThreadPriority.AboveNormal;
            _initInvoicesThread.Start();
        }

        private void InitInvoices()
        {
            Thread _initInvoicesThread = new Thread(ThreadTaskGenerateDebugDataRecords);
            _initInvoicesThread.Priority = ThreadPriority.AboveNormal;
            _initInvoicesThread.Start();
        }

        private void InitOrganization()
        {
            Thread _initOrganizationsThread = new Thread(ThreadTaskInitOrganization);
            _initOrganizationsThread.Priority = ThreadPriority.Normal;
            _initOrganizationsThread.Start();
        }

        private void RefreshDataGrid()
        {
            Thread _refreshDataGridThread = new Thread(ThreadTaskRefreshDataGrid);
            _refreshDataGridThread.Priority = ThreadPriority.Normal;
            _refreshDataGridThread.Start();
        }

        private void ThreadTaskGenerateDebugDataRecords()
        {
            Random r = new Random();
            string[] sampleOrganization = { "UPS", "MCDonalds", "Telekom", "DHL", "Amazon", "Apple", "Microsoft", "Google", "Facebook", "Twitter" };

                //Invoices debug records
                Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(()
                    => { Dg_Invoices.Items.Clear(); }));

                for (int i = 0; i < 5; i++)
                {
                    InvoiceModel invoice = new InvoiceModel();
                    invoice.Reference = "REF" + r.Next(1000, 9999);
                    invoice.InvoiceNumber = "INV" + r.Next(1000, 9999);
                    invoice.Organization = sampleOrganization[r.Next(0, sampleOrganization.Length)];
                    invoice.ExhibitionDate = DateTime.Now.AddDays(r.Next(0, 100));
                    invoice.Path = "C:\\Users\\Schecher_1\\Desktop\\Test.pdf";

                    //Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        //=> { Dg_Invoices.Items.Add(invoice); }));
                    allInvoices.Add(invoice);
                }
        }

        private void ThreadTaskInitInvoices()
        {
            
        }
        
        private void ThreadTaskInitOrganization()
        {
                Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(()
                     => { Comb_Search_Organization.Items.Clear(); }));

                foreach (var organization in allInvoices.Select(x => x.Organization).Distinct())
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { Comb_Search_Organization.Items.Add(organization); }));
        }

        private void ThreadTaskRefreshDataGrid()
        {
            //run this as task, because the sorting can take a while
            SortSystem sortSys = new SortSystem(allInvoices, filterReference, filterInvoiceNumber, filterOrganization, filterExhibitionDate);

            List<InvoiceModel> filteredInvoices = sortSys.Sort();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                => { Dg_Invoices.Items.Clear(); }));

            foreach (var invoice in filteredInvoices)
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                    => { Dg_Invoices.Items.Add(invoice); }));

        }

        private void DG_Invoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            => Process.Start(pathPDFBrowser, ((InvoiceModel)Dg_Invoices.SelectedItem).Path);

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

        private void Comb_Search_Organization_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_Organization.SelectedIndex = -1;
    }
}
