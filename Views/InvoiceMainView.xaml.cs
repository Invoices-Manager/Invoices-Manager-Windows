﻿using InvoicesManager.Core.Sort;

namespace InvoicesManager.Views
{
    public partial class InvoiceMainView : Page
    {
        //the last selected items vars
        private string lastOrganization = String.Empty;
        private string lastDocumentType = String.Empty;

        //the filter vars
        private string filterReference = String.Empty;
        private string filterInvoiceNumber = String.Empty;
        private string filterOrganization = "-1";
        private string filterDocumentType = "-1";
        private DateTime filterExhibitionDate = default;
        private string filterTags = String.Empty;
        private PaidStateEnum filterPaidState = PaidStateEnum.FilterPlaceholder;
        private MoneyStateEnum filterMoneyState = MoneyStateEnum.FilterPlaceholder;
        private ImportanceStateEnum filterImportanceState = ImportanceStateEnum.FilterPlaceholder;
        private double filterMoneyTotal = double.MinValue; // -1 is not possible because it is a valid value

        public InvoiceMainView()
        {
#if DEBUG
            // try { File.Delete(EnvironmentsVariable.PathData + EnvironmentsVariable.InvoicesJsonFileName); } catch  {}
#endif
            //load the window 
            InitializeComponent();
            //set the column visibility
            SetColumnVisibility();
            //init threads
            InitThreads();
            //check for auto backup
            if (EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts)
            {
                Task.Run(() =>
                {
                    LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.MainWindow_View, "auto backup was requested");
                    BackUpSystem buSys = new BackUpSystem();
                    bool wasPerformedCorrectly = buSys.BackUp(Path.Combine(EnvironmentsVariable.PathBackUps, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bkup"));
                    if (!wasPerformedCorrectly)
                    {
                        LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.MainWindow_View, "the requested auto backup failed");
                        MessageBox.Show(this.Resources["backUpFailed"] as string);
                    }
                    buSys.CheckBackUpCount();
                });
            }

#if DEBUG
            //GenerateDebugDataRecords();
            //RefreshDataGridWithInit();
#endif
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
            => RefreshDataGridWithInit();

        private void GenerateDebugDataRecords()
        {
            Random r = new Random();
            string[] sampleOrganization = { "UPS", "MCDonalds", "Telekom", "DHL", "Amazon", "Apple", "Microsoft", "Google", "Facebook", "Twitter" };
            string[] sampleDocumenttype = { "Invoice", "Bill" };
            List<InvoiceModel> sampleInvoices = new List<InvoiceModel>();

            for (int i = 0; i < r.Next(35, 125); i++)
            {
                InvoiceModel invoice = new InvoiceModel();
                invoice.FileID = "test";
                invoice.CaptureDate = DateTime.Now.AddDays(r.Next(-100, 100));
                invoice.ExhibitionDate = DateTime.Now.AddDays(r.Next(-100, 100));
                invoice.Reference = "REF-" + r.Next(100000, 999999).ToString();
                invoice.DocumentType = sampleDocumenttype[r.Next(0, sampleDocumenttype.Length)];
                invoice.Organization = sampleOrganization[r.Next(0, sampleOrganization.Length)];
                invoice.InvoiceNumber = "INV-NR" + r.Next(100000, 999999).ToString();
                invoice.Tags = new string[] { "Tag1", "Tag2", "Tag3" };
                invoice.MoneyTotal = r.Next(100, 9999);
                invoice.ImportanceState = (ImportanceStateEnum)r.Next(0, 3);
                invoice.MoneyState = (MoneyStateEnum)r.Next(0, 2);
                invoice.PaidState = (PaidStateEnum)r.Next(0, 2);

                sampleInvoices.Add(invoice);
            }

            File.WriteAllText(EnvironmentsVariable.PathInvoices + EnvironmentsVariable.InvoicesJsonFileName, JsonConvert.SerializeObject(sampleInvoices, Formatting.Indented));
        }

        private void InitThreads()
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.MainWindow_View, "init threads was requested");
#endif

            InvoiceSystem iSys = new InvoiceSystem();

            Thread _initInvoicesThread = new Thread(iSys.Init);
            Thread _initOrganizationsThread = new Thread(ThreadTaskInitOrganization);
            Thread _initDocumentType = new Thread(ThreadTaskInitDocumentType);
            Thread _refreshDataGridThread = new Thread(ThreadTaskRefreshDataGrid);
            Thread _initPaidStateThread = new Thread(ThreadTaskInitPaidState);
            Thread _initMoneyStateThread = new Thread(ThreadTaskInitMoneyState);
            Thread _initImportanceState = new Thread(ThreadTaskInitImportanceState);

            _initInvoicesThread.Start();
            _initInvoicesThread.Join();

            _initOrganizationsThread.Start();
            _initOrganizationsThread.Join();

            _initDocumentType.Start();
            _initDocumentType.Join();

            _refreshDataGridThread.Start();
            _refreshDataGridThread.Join();

            _initPaidStateThread.Start();
            _initPaidStateThread.Join();

            _initMoneyStateThread.Start();
            _initMoneyStateThread.Join();

            _initImportanceState.Start();
            _initImportanceState.Join();
        }

        private void InitInvoices()
        {
            InvoiceSystem iSys = new InvoiceSystem();

            Thread _initInvoicesThread = new Thread(iSys.Init);
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
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.MainWindow_View, "RefreshDataGridWithInit was requested");
#endif
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

            //it must be done, otherwise the exception will be thrown (
            //System.InvalidOperationException: "Collection was modified; enumeration operation may not execute".)
            List<InvoiceModel> allInvoices = new List<InvoiceModel>(EnvironmentsVariable.AllInvoices);

            //otherwise an empty orga will be displayed in the comb
            allInvoices.RemoveAll(x => x.Organization == String.Empty);

            foreach (var organization in allInvoices
                .Select(x => x.Organization)
                .Distinct()
                .OrderBy(x => x))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { Comb_Search_Organization.Items.Add(organization); }));

            //sets the last selected organization if it is still available
            if (Comb_Search_Organization.Items.Contains(lastOrganization))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        =>{ Comb_Search_Organization.SelectedItem = lastOrganization; }));
        }

        private void ThreadTaskInitPaidState()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                     => { Comb_Search_PaidState.Items.Clear(); }));


            foreach (PaidStateEnum pse in Enum.GetValues(typeof(PaidStateEnum)))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { if (pse != PaidStateEnum.FilterPlaceholder)
                                Comb_Search_PaidState.Items.Add(PaidState.EnumAsString(pse));}));
        }

        private void ThreadTaskInitMoneyState()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                     => { Comb_Search_MoneyState.Items.Clear(); }));


            foreach (MoneyStateEnum mse in Enum.GetValues(typeof(MoneyStateEnum)))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => {if (mse != MoneyStateEnum.FilterPlaceholder)
                                Comb_Search_MoneyState.Items.Add(MoneyState.EnumAsString(mse));}));
        }

        private void ThreadTaskInitImportanceState()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                     => { Comb_Search_ImportanceState.Items.Clear(); }));

            foreach (ImportanceStateEnum ise in Enum.GetValues(typeof(ImportanceStateEnum)))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { if (ise != ImportanceStateEnum.FilterPlaceholder)
                                Comb_Search_ImportanceState.Items.Add(ImportanceState.EnumAsString(ise)); }));
        }

        private void ThreadTaskInitDocumentType()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                => { Comb_Search_DocumentType.Items.Clear(); }));

            foreach (var documenttype in EnvironmentsVariable.AllInvoices
                .Select(x => x.DocumentType)
                .Distinct()
                .OrderBy(x => x))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                    => { Comb_Search_DocumentType.Items.Add(documenttype); }));

            //sets the last selected document type if it is still available
            if (Comb_Search_DocumentType.Items.Contains(lastDocumentType))
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { Comb_Search_DocumentType.SelectedItem = lastDocumentType; }));
        }

        private void ThreadTaskRefreshDataGrid()
        {
            //sleep to wait for the init thread
            WaiterSystem.WaitUntilInvoiceInitFinish();
            
            InvoicesSortSystem sortSys = new Core.Sort.InvoicesSortSystem(EnvironmentsVariable.AllInvoices, filterReference, filterInvoiceNumber, filterOrganization, filterDocumentType, filterExhibitionDate, filterPaidState, filterMoneyState, filterImportanceState, filterMoneyTotal, filterTags);

            sortSys.Sort();

            //Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
            //    => { Dg_Invoices.Items.Clear(); }));

            //foreach (var invoice in EnvironmentsVariable.FilteredInvoices)
            //    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
            //        => { Dg_Invoices.Items.Add(invoice); }));

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                => { Dg_Invoices.ItemsSource = EnvironmentsVariable.FilteredInvoices; }));

            //set bottom status bar
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                string wordInvoice = Application.Current.Resources["invoices"] as string;
                var wordFrom = Application.Current.Resources["from"] as string;
                MsgBox_InvoiceCounter.Content = $"{wordInvoice}:  {EnvironmentsVariable.FilteredInvoices.Count} {wordFrom} {EnvironmentsVariable.AllInvoices.Count}";
            }));
        }

        private void DG_Invoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //happens sometimes, therefore this catch
            if (Dg_Invoices.SelectedItem == null)
                return;

            var cellInfo = Dg_Invoices.CurrentCell;
            var column = cellInfo.Column as DataGridBoundColumn;

            //if the colum is the Resources["open"]
            // it will be open the file
            //      else
            // you copy the column value into you Clipboard
            if (column.Header as string != Application.Current.Resources["open"] as string)
            {
                var element = new FrameworkElement() { DataContext = cellInfo.Item };
                BindingOperations.SetBinding(element, TagProperty, column.Binding);
                var cellValue = element.Tag;
                Clipboard.SetText(cellValue.ToString());

                //must, otherwise you save it in your clipboard and open the file
                return;
            }

            InvoiceModel invoice = Dg_Invoices.SelectedItem as InvoiceModel;

            //copy file to temp folder and open it then delete it
            string tempPath = Path.Combine(Path.GetTempPath(), invoice.FileID + ".pdf");
            string sourcePath = Path.Combine(EnvironmentsVariable.PathInvoices, invoice.FileID + ".pdf");
            File.Copy(sourcePath, tempPath, true);
            Process.Start(EnvironmentsVariable.PathPDFBrowser, tempPath);

            //this program has to wait, so the pdf browser can open it
            //otherwise he is faster with delete than the document can be displayed
            Thread.Sleep(1000);

            File.Delete(tempPath);
        }

        private void Bttn_BoardRefresh_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentsVariable.InitWorkPath();
            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenInvoiceView(InvoiceViewModeEnum.InvoiceAdd);

            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;

            OpenInvoiceView(InvoiceViewModeEnum.InvoiceEdit);

            RefreshDataGridWithInit();
        }

        private void Bttn_InvoiceRemove_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;

            OpenInvoiceView(InvoiceViewModeEnum.InvoiceDelete);

            RefreshDataGridWithInit();
        }

        private void OpenInvoiceView(InvoiceViewModeEnum invoiceViewModeEnum)
        {
            InvoiceViewWindow invoiceViewWindow = new InvoiceViewWindow(invoiceViewModeEnum, (InvoiceModel)Dg_Invoices.SelectedItem);
            invoiceViewWindow.ShowDialog();
        }

        private void Bttn_InvoiceSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Invoices.SelectedItem == null)
                return;

            InvoiceSaveAsWindow _invoiceSaveAsWindow = new InvoiceSaveAsWindow((InvoiceModel)Dg_Invoices.SelectedItem);
            _invoiceSaveAsWindow.ShowDialog();
        }

        private void Tb_Search_String_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterReference = Tb_Search_String.Text == String.Empty ? String.Empty : Tb_Search_String.Text;
            RefreshDataGrid();
        }

        private void Tb_Tags_String_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterTags = Tb_Tags_String.Text == String.Empty ? String.Empty : Tb_Tags_String.Text;
            RefreshDataGrid();
        }

        private void Tb_Search_InvoiceNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterInvoiceNumber = Tb_Search_InvoiceNumber.Text == String.Empty ? String.Empty : Tb_Search_InvoiceNumber.Text;
            RefreshDataGrid();
        }

        private void Tb_Search_MoneyTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            //check if the call was for the refresh
            if (Tb_Search_MoneyTotal.Text == String.Empty)
            {
                filterMoneyTotal = Double.MinValue;
                RefreshDataGrid();
                return;
            }

            //check if the input is a double
            if (!double.TryParse(Tb_Search_MoneyTotal.Text, out double moneyTotal))
            {
                Tb_Search_MoneyTotal.Text = String.Empty;
                return;
            }

            filterMoneyTotal = Tb_Search_MoneyTotal.Text == String.Empty ? Double.MinValue : Convert.ToDouble(Tb_Search_MoneyTotal.Text);
            RefreshDataGrid();
        }

        private void Comb_Search_Organization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //saves the last selected item, for more infos see changelog v1.4.1.0 (RefreshGrid) 
            if (Comb_Search_Organization.SelectedIndex.ToString() != "-1")
                lastOrganization = Comb_Search_Organization.SelectedItem.ToString();

            filterOrganization = Comb_Search_Organization.SelectedIndex.ToString() == "-1" ? "-1" : Comb_Search_Organization.SelectedItem.ToString();
            RefreshDataGrid();
        }

        private void Comb_Search_Organization_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_Organization.SelectedIndex = -1;

        private void Comb_Search_DocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //saves the last selected item, for more infos see changelog v1.4.1.0 (RefreshGrid) 
            if (Comb_Search_DocumentType.SelectedIndex.ToString() != "-1")
                lastDocumentType = Comb_Search_DocumentType.SelectedItem.ToString();

            filterDocumentType = Comb_Search_DocumentType.SelectedIndex.ToString() == "-1" ? "-1" : Comb_Search_DocumentType.SelectedItem.ToString();
            RefreshDataGrid();
        }

        private void Comb_Search_ImportanceState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterImportanceState = Comb_Search_ImportanceState.SelectedIndex.ToString() == "-1" ? ImportanceStateEnum.FilterPlaceholder : ImportanceState.StringAsEnum(Comb_Search_ImportanceState.SelectedItem.ToString());
            RefreshDataGrid();
        }

        private void Comb_Search_MoneyState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterMoneyState = Comb_Search_MoneyState.SelectedIndex.ToString() == "-1" ? MoneyStateEnum.FilterPlaceholder : MoneyState.StringAsEnum(Comb_Search_MoneyState.SelectedItem.ToString());
            RefreshDataGrid();
        }

        private void Comb_Search_PaidState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterPaidState = (Comb_Search_PaidState.SelectedIndex.ToString() == "-1" ? PaidStateEnum.FilterPlaceholder : PaidState.StringAsEnum(Comb_Search_PaidState.SelectedItem.ToString()));
            RefreshDataGrid();
        }

        private void Dp_Search_ExhibitionDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterExhibitionDate = (DateTime)(Dp_Search_ExhibitionDate.SelectedDate == null ? default(DateTime) : Dp_Search_ExhibitionDate.SelectedDate);
            RefreshDataGrid();
        }

        private void Tb_Search_Tags_Clear_Click(object sender, RoutedEventArgs e)
            => Tb_Tags_String.Text = String.Empty;

        private void Comb_Search_DocumentType_Clear_Click(object sender, RoutedEventArgs e)
             => Comb_Search_DocumentType.SelectedIndex = -1;

        private void Comb_Search_PaidState_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_PaidState.SelectedIndex = -1;

        private void Comb_Search_MoneyState_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_MoneyState.SelectedIndex = -1;

        private void Comb_Search_ImportanceState_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_ImportanceState.SelectedIndex = -1;

        private void Dp_Search_ExhibitionDate_Clear_Click(object sender, RoutedEventArgs e)
           => Dp_Search_ExhibitionDate.SelectedDate = null;

        private void Tb_Search_MoneyTotal_Clear_Click(object sender, RoutedEventArgs e)
            => Tb_Search_MoneyTotal.Text = String.Empty;

        private void Tb_Search_InvoiceNumber_Clear_Click(object sender, RoutedEventArgs e)
            => Tb_Search_InvoiceNumber.Text = String.Empty;

        private void Tb_Search_String_Clear_Click(object sender, RoutedEventArgs e)
            => Tb_Search_String.Text = String.Empty;

        private void Dg_Invoices_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            => Dg_Invoices.ContextMenu.IsOpen = true;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "ViewColumnOpen":
                    ColumnOpen.Visibility = ColumnOpen.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnOpen.IsChecked = ColumnOpen.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnDateOfExhibition":
                    ColumnDateOfExhibition.Visibility = ColumnDateOfExhibition.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnDateOfExhibition.IsChecked = ColumnDateOfExhibition.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnOrganization":
                    ColumnOrganization.Visibility = ColumnOrganization.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnOrganization.IsChecked = ColumnOrganization.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnDocumentType":
                    ColumnDocumentType.Visibility = ColumnDocumentType.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnDocumentType.IsChecked = ColumnDocumentType.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnInvoiceNo":
                    ColumnInvoiceNo.Visibility = ColumnInvoiceNo.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnInvoiceNo.IsChecked = ColumnInvoiceNo.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnReference":
                    ColumnReference.Visibility = ColumnReference.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnReference.IsChecked = ColumnReference.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnMoneyTotal":
                    ColumnMoneyTotal.Visibility = ColumnMoneyTotal.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnMoneyTotal.IsChecked = ColumnMoneyTotal.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnImportanceState":
                    ColumnImportanceState.Visibility = ColumnImportanceState.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnImportanceState.IsChecked = ColumnImportanceState.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnMoneyState":
                    ColumnMoneyState.Visibility = ColumnMoneyState.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnMoneyState.IsChecked = ColumnMoneyState.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnPaidState":
                    ColumnPaidState.Visibility = ColumnPaidState.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnPaidState.IsChecked = ColumnPaidState.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnTags":
                    ColumnTags.Visibility = ColumnTags.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnTags.IsChecked = ColumnTags.Visibility == Visibility.Visible;
                    break;

                case "ViewColumnDateOfCapture":
                    ColumnDateOfCapture.Visibility = ColumnDateOfCapture.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    ViewColumnDateOfCapture.IsChecked = ColumnDateOfCapture.Visibility == Visibility.Visible;
                    break;
            }

            //save the current state of the columns
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnOpen = ColumnOpen.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnDateOfExhibition = ColumnDateOfExhibition.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnOrganization = ColumnOrganization.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnDocumentType = ColumnDocumentType.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnInvoiceNo = ColumnInvoiceNo.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnReference = ColumnReference.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnMoneyTotal = ColumnMoneyTotal.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnImportanceState = ColumnImportanceState.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnMoneyState = ColumnMoneyState.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnPaidState = ColumnPaidState.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnTags = ColumnTags.Visibility == Visibility.Visible;
            EnvironmentsVariable.ColumnVisibility.IsVisibleColumnDateOfCapture = ColumnDateOfCapture.Visibility == Visibility.Visible;

            //save into the config file
            ConfigSystem _cs = new ConfigSystem();
            _cs.SaveIntoJsonFile();
        }

        private void SetColumnVisibility()
        {
            SetColumnVisibility(ColumnOpen, ViewColumnOpen, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnOpen);
            SetColumnVisibility(ColumnDateOfExhibition, ViewColumnDateOfExhibition, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnDateOfExhibition);
            SetColumnVisibility(ColumnOrganization, ViewColumnOrganization, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnOrganization);
            SetColumnVisibility(ColumnDocumentType, ViewColumnDocumentType, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnDocumentType);
            SetColumnVisibility(ColumnInvoiceNo, ViewColumnInvoiceNo, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnInvoiceNo);
            SetColumnVisibility(ColumnReference, ViewColumnReference, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnReference);
            SetColumnVisibility(ColumnMoneyTotal, ViewColumnMoneyTotal, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnMoneyTotal);
            SetColumnVisibility(ColumnImportanceState, ViewColumnImportanceState, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnImportanceState);
            SetColumnVisibility(ColumnMoneyState, ViewColumnMoneyState, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnMoneyState);
            SetColumnVisibility(ColumnPaidState, ViewColumnPaidState, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnPaidState);
            SetColumnVisibility(ColumnTags, ViewColumnTags, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnTags);
            SetColumnVisibility(ColumnDateOfCapture, ViewColumnDateOfCapture, EnvironmentsVariable.ColumnVisibility.IsVisibleColumnDateOfCapture);
        }

        private void SetColumnVisibility(DataGridTextColumn column, MenuItem checkBox, bool isVisible)
        {
            column.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            checkBox.IsChecked = isVisible;
        }
    }
}
