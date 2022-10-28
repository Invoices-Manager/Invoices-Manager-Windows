using InvoicesManager.Classes;
using InvoicesManager.Core;
using InvoicesManager.Models;
using InvoicesManager.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace InvoicesManager
{
    public partial class MainWindow : Window
    {
        private string filterReference = String.Empty;
        private string filterInvoiceNumber = String.Empty;
        private string filterOrganization = "-1";
        private string filterDocumentType = "-1";
        private DateTime filterExhibitionDate = default;

        public MainWindow()
        {
#if DEBUG_WITHDEBUGRECORDS || DEBUG
            try { File.Delete(EnvironmentsVariable.PathData + EnvironmentsVariable.InvoicesJsonFileName); } catch  {}
#endif

            //init work path
            EnvironmentsVariable.InitWorkPath();
            //load the settings
            SettingSystem.Init();
            //load the window UI language
            LanguageManager.Init();
            //load the window 
            InitializeComponent();
            //init threads
            InitThreads();
            
#if DEBUG
            GenerateDebugDataRecords();
            RefreshDataGridWithInit();
#endif
        }

        private void GenerateDebugDataRecords()
        {
            Random r = new Random();
            string[] sampleOrganization = { "UPS", "MCDonalds", "Telekom", "DHL", "Amazon", "Apple", "Microsoft", "Google", "Facebook", "Twitter" };
            string[] sampleDocumenttype = { "Invoice", "Bill" };

            for (int i = 0; i < r.Next(35, 125); i++)
            {
                InvoiceModel invoice = new InvoiceModel();
                invoice.Reference = "REF" + r.Next(1000, 9999);
                invoice.InvoiceNumber = "INV" + r.Next(1000, 9999);
                invoice.DocumentType = sampleDocumenttype[r.Next(0, sampleDocumenttype.Length)];
                invoice.Organization = sampleOrganization[r.Next(0, sampleOrganization.Length)];
                invoice.ExhibitionDate = DateTime.Now.AddDays(r.Next(0, 100));
                invoice.Path = "C:\\Invoices\\" + invoice.Reference + ".pdf";

                EnvironmentsVariable.allInvoices.Add(invoice);
            }

            File.WriteAllText(EnvironmentsVariable.PathData + EnvironmentsVariable.InvoicesJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.allInvoices));
        }

        private void InitThreads()
        {
            Thread _initInvoicesThread = new Thread(InvoiceSystem.Init);
            Thread _initOrganizationsThread = new Thread(ThreadTaskInitOrganization);
            Thread _initDocumentType = new Thread(ThreadTaskInitDocumentType);
            Thread _refreshDataGridThread = new Thread(ThreadTaskRefreshDataGrid);

            _initInvoicesThread.Start();
            _initInvoicesThread.Join();

            _initOrganizationsThread.Start();
            _initOrganizationsThread.Join();

            _initDocumentType.Start();
            _initDocumentType.Join();

            _refreshDataGridThread.Start();
            _refreshDataGridThread.Join();
        }

        private void InitInvoices()
        {
            Thread _initInvoicesThread = new Thread(InvoiceSystem.Init);
            _initInvoicesThread.Priority = ThreadPriority.Highest;
            _initInvoicesThread.Start();
        }

        private void InitOrganization()
        {
            Thread _initOrganizationsThread = new Thread(ThreadTaskInitOrganization);
            _initOrganizationsThread.Priority = ThreadPriority.Normal;
            _initOrganizationsThread.Start();
        }

        private void InitDocumentType()
        {
            Thread _initDocumentType = new Thread(ThreadTaskInitDocumentType);
            _initDocumentType.Priority = ThreadPriority.Normal;
            _initDocumentType.Start();
        }

        private void RefreshDataGrid()
        {
            Thread _refreshDataGridThread = new Thread(ThreadTaskRefreshDataGrid);
            _refreshDataGridThread.Priority = ThreadPriority.Normal;
            _refreshDataGridThread.Start();
        }

        private void RefreshDataGridWithInit()
        {
            InitInvoices();
            InitOrganization();
            InitDocumentType();
            RefreshDataGrid();
        }

        private void ThreadTaskInitOrganization()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                     => { Comb_Search_Organization.Items.Clear(); }));

                foreach (var organization in EnvironmentsVariable.allInvoices.Select(x => x.Organization).Distinct())
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { Comb_Search_Organization.Items.Add(organization); }));
        }

        private void ThreadTaskInitDocumentType()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                => { Comb_Search_DocumentType.Items.Clear(); }));

            foreach (var documenttype in EnvironmentsVariable.allInvoices.Select(x => x.DocumentType).Distinct())
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                    => { Comb_Search_DocumentType.Items.Add(documenttype); }));
        }

        private void ThreadTaskRefreshDataGrid()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            SortSystem sortSys = new SortSystem(EnvironmentsVariable.allInvoices, filterReference, filterInvoiceNumber, filterOrganization, filterDocumentType , filterExhibitionDate);

            sortSys.Sort();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                => { Dg_Invoices.Items.Clear(); }));
            
            foreach (var invoice in EnvironmentsVariable.filteredInvoices)
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                    => { Dg_Invoices.Items.Add(invoice); }));

            //set bottom status bar
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                string wordInvoice = Application.Current.Resources["invoices"] as string;
                var wordFrom = Application.Current.Resources["from"] as string;
                MsgBox_InvoiceCounter.Content = $"{wordInvoice}:  {EnvironmentsVariable.filteredInvoices.Count} {wordFrom} {EnvironmentsVariable.allInvoices.Count}";
            }));
        }
        
        private void DG_Invoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;

            InvoiceModel invoice = (InvoiceModel)Dg_Invoices.SelectedItem;
            Process.Start(EnvironmentsVariable.PathPDFBrowser, $"\"{invoice.Path}\"");
        }

        private void Bttn_BoardRefresh_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentsVariable.InitWorkPath();
            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            InvoiceAddWindow _invoiceAddWindow = new InvoiceAddWindow();
            _invoiceAddWindow.ShowDialog();
            
            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;

            InvoiceEditWindow _invoiceEditWindow = new InvoiceEditWindow((InvoiceModel)Dg_Invoices.SelectedItem);
            _invoiceEditWindow.ShowDialog();

            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceRemove_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;
            
            InvoiceRemoveWindow _invoiceRemoveWindow = new InvoiceRemoveWindow((InvoiceModel)Dg_Invoices.SelectedItem);
            _invoiceRemoveWindow.ShowDialog();
            
            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;

            InvoiceSaveAsWindow _invoiceSaveAsWindow = new InvoiceSaveAsWindow((InvoiceModel)Dg_Invoices.SelectedItem);
            _invoiceSaveAsWindow.ShowDialog();
        }

        private async void Bttn_BackUpCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog()
            {
                Filter = "BackUp-Datei (*.bkup)|*.bkup",
                RestoreDirectory = true
            };

            if (sfg.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    if (BackUpSystem.BackUp(sfg.FileName))
                        MessageBox.Show(Application.Current.Resources["backUpSuccessfully"] as string);
                    else
                        MessageBox.Show(Application.Current.Resources["backUpFailed"] as string);
                });
            }
        }

        private async void Bttn_BackUpRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "BackUp-Datei (*.bkup)|*.bkup"
            };

            if (ofd.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    if (!BackUpSystem.Restore(ofd.FileName))
                        MessageBox.Show(this.Resources["backUpFailedRestored"] as string);

                    RefreshDataGridWithInit();
                });
            }
        }

        private void Bttn_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow _settingWindow = new SettingWindow();
            _settingWindow.ShowDialog();

            RefreshDataGridWithInit();
        }

        private void Bttn_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow _aboutWindow = new AboutWindow();
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

        private void Comb_Search_Organization_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_Organization.SelectedIndex = -1;

        private void Comb_Search_DocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterDocumentType = Comb_Search_DocumentType.SelectedIndex.ToString() == "-1" ? "-1" : Comb_Search_DocumentType.SelectedItem.ToString();
            RefreshDataGrid();
        }

        private void Comb_Search_DocumentType_Clear_Click(object sender, RoutedEventArgs e)
             => Comb_Search_DocumentType.SelectedIndex = -1;

        private void Dp_Search_ExhibitionDate_Clear_Click(object sender, RoutedEventArgs e)
           => Dp_Search_ExhibitionDate.SelectedDate = null;

        private void Dp_Search_ExhibitionDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterExhibitionDate = (DateTime)(Dp_Search_ExhibitionDate.SelectedDate == null ? default(DateTime) : Dp_Search_ExhibitionDate.SelectedDate);
            RefreshDataGrid();
        }
    }
}
