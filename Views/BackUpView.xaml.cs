namespace InvoicesManager.Views
{
    public partial class BackUpView: Page
    {
        public BackUpView()
        {
            InitializeComponent();
            RefreshDataGrid();
        }

        private void Bttn_BoardRefresh_Click(object sender, RoutedEventArgs e)
            => RefreshDataGrid();

        private async void Bttn_BackUpCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                RestoreDirectory = true
            };

            if (sfg.ShowDialog() == DialogResult.OK)
            {
                await Task.Run(() =>
                {
                    LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "BackUp was requested");
                    BackUpSystem buSys = new BackUpSystem();
                    if (buSys.BackUp(sfg.FileName))
                    {
                        LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "BackUp was completed successfully!");
                        MessageBox.Show(Application.Current.Resources["backUpSuccessfully"] as string);
                    }
                    else
                    {
                        LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "BackUp was not completed successfully!");
                        MessageBox.Show(Application.Current.Resources["backUpFailed"] as string);
                    }
                });
            }
        }

        private async void Bttn_BackUpRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                await Task.Run(() =>
                {
                    LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "Restore was requested");
                    BackUpSystem buSys = new BackUpSystem();
                    if (buSys.Restore(ofd.FileName))
                    {
                        LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "Restore was completed successfully!");
                    }
                    else
                    {
                        LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "Restore was not completed successfully!");
                        MessageBox.Show(Application.Current.Resources["backUpFailedRestored"] as string);
                    }
                });
            }
        }

        private async void Bttn_BackUpSaveAs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd_backUpFilePath = new OpenFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                CheckFileExists = true
            };

            OpenFileDialog ofd_savePath = new OpenFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                CheckFileExists = false
            };

            if (ofd_backUpFilePath.ShowDialog() != DialogResult.OK)
                return;

            if (ofd_savePath.ShowDialog() != DialogResult.OK)
                return;

            await Task.Run(() =>
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "Save as was requested");
                BackUpSystem buSys = new BackUpSystem();
                if (buSys.SaveAs(ofd_backUpFilePath.FileName, ofd_savePath.FileName))
                {
                    LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "Save as was completed successfully!");
                }
                else
                {
                    LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "Save as was not completed successfully!");
                    MessageBox.Show(Application.Current.Resources["backUpFailedSaveAs"] as string);
                }
            });
        }
        
        private async void RefreshDataGrid()
        {
            await Task.Run(() =>
            {
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, "Refresh was requested");
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, "GetBackUps() was requested");
                BackUpSystem buSys = new BackUpSystem();
                List<BackUpInfoModel> backUpInfos = buSys.GetBackUps();
                if (backUpInfos != null)
                {
                    LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, $"Refresh was completed successfully! {backUpInfos.Count} items were found");
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { 
                                Dg_BackUps.ItemsSource = backUpInfos;
                                MsgBox_BackUpCounter.Content = $"{Application.Current.Resources["backUpCount"] as string}: {backUpInfos.Count}";
                            }));
                }
                else
                {
                    LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "Refresh was not completed successfully!");
                }
            });
        }
    }
}
