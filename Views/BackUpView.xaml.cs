namespace InvoicesManager.Views
{
    public partial class BackUpView: Page
    {
        public BackUpView()
        {
            InitializeComponent();
            _ = RefreshDataGrid();
        }

        private void Bttn_BoardRefresh_Click(object sender, RoutedEventArgs e)
            => _ = RefreshDataGrid();

        private async void Bttn_BackUpCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                RestoreDirectory = true
            };

            if (sfg.ShowDialog() == DialogResult.OK)
                return;

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

        private async void Bttn_BackUpRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

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

        private async Task RefreshDataGrid()
        {
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, "Refresh was requested");
            BackUpSystem buSys = new BackUpSystem();

            //clear the dg
            Dg_BackUps.Items.Clear();

            await foreach (BackUpInfoModel backUpInfo in buSys.GetBackUps())
            {
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, $"New BackUpInfoModel was found: {backUpInfo.BackUpName}");
                Dg_BackUps.Items.Add(backUpInfo);
                MsgBox_BackUpCounter.Content = $"{Application.Current.Resources["backUpCount"] as string}: {Dg_BackUps.Items.Count}";
            }

            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, $"Refresh was completed successfully! {Dg_BackUps.Items.Count} items were found");
        }

        private void MenuItem_BackUpRestore_Click(object sender, RoutedEventArgs e)
        {
            BackUpInfoModel selectedBackUp = (BackUpInfoModel)Dg_BackUps.SelectedItem;
            BackUpSystem buSys = new BackUpSystem();

            buSys.Restore(selectedBackUp.BackUpPath);
        }

        private void MenuItem_BackSaveAs_Click(object sender, RoutedEventArgs e)
        {
            BackUpInfoModel selectedBackUp = (BackUpInfoModel)Dg_BackUps.SelectedItem;
            BackUpSystem buSys = new BackUpSystem();
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                CheckFileExists = false
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            buSys.SaveAs(selectedBackUp.BackUpPath, ofd.FileName);
        }

        private void MenuItem_BackUpDelete_Click(object sender, RoutedEventArgs e)
        {
            BackUpInfoModel selectedBackUp = (BackUpInfoModel)Dg_BackUps.SelectedItem;
            BackUpSystem buSys = new BackUpSystem();
            
            LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "BackUp delete was requested");

            if (buSys.Delete(selectedBackUp.BackUpPath))
            {
                Dg_BackUps.Items.Remove(selectedBackUp);
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, $"BackUp was deleted {selectedBackUp.BackUpName}");
            }
            else
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.BackUp_View, $"BackUp was  NOT deleted {selectedBackUp.BackUpName}");
        }
    }
}
