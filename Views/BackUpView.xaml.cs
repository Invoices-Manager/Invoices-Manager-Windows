using System.Security.Cryptography;

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
            await Task.Run(() =>
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, $"BackUp was requested {EnvironmentsVariable.PathBackUps}");
                BackUpSystem buSys = new BackUpSystem();
                if (buSys.BackUp(Path.Combine(EnvironmentsVariable.PathBackUps, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bkup"), this))
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
            await RefreshDataGrid();
        }
        
        private async void Bttn_BackUpCreateSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                RestoreDirectory = true
            };

            if (sfg.ShowDialog() != DialogResult.OK)
                return;

            await Task.Run(() =>
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, $"BackUp was requested {sfg.FileName}");
                BackUpSystem buSys = new BackUpSystem();
                if (buSys.BackUp(sfg.FileName, this))
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
                if (buSys.Restore(ofd.FileName, this))
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

        private async Task RefreshDataGrid()
        {
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, "Refresh was requested");
            BackUpSystem buSys = new BackUpSystem();

            //clear the dg and counter
            Dg_BackUps.Items.Clear();
            MsgBox_BackUpCounter.Content = $"{Application.Current.Resources["backUpCount"] as string}: 0";

            await foreach (BackUpInfoModel backUpInfo in buSys.GetBackUps())
            {
                LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, $"New BackUpInfoModel was found: {backUpInfo.BackUpName}");
                Dg_BackUps.Items.Add(backUpInfo);
                MsgBox_BackUpCounter.Content = $"{Application.Current.Resources["backUpCount"] as string}: {Dg_BackUps.Items.Count}";
            }

            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.BackUp_View, $"Refresh was completed successfully! {Dg_BackUps.Items.Count} items were found");
        }

        private async void MenuItem_BackUpRestore_Click(object sender, RoutedEventArgs e)
        {
            BackUpInfoModel selectedBackUp = (BackUpInfoModel)Dg_BackUps.SelectedItem;
            BackUpSystem buSys = new BackUpSystem();

            if (selectedBackUp is null)
            {
                LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "Restore was requested, but backUp was null!");
                return;
            }

            LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, $"Restore was requested {selectedBackUp.BackUpPath}");

            await Task.Run(() =>
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "Restore was requested");
                BackUpSystem buSys = new BackUpSystem();
                if (buSys.Restore(selectedBackUp.BackUpPath, this))
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

        private void MenuItem_BackSaveAs_Click(object sender, RoutedEventArgs e)
        {
            BackUpInfoModel selectedBackUp = (BackUpInfoModel)Dg_BackUps.SelectedItem;
            BackUpSystem buSys = new BackUpSystem();
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = Application.Current.Resources["bkupFilter"] as string,
                CheckFileExists = false
            };

            if (selectedBackUp is null)
            {
                LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "BackUp save as was requested, but backUp was null!");
                return;
            }

            LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, $"BackUp save as was requested  {ofd.FileName}");

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (buSys.SaveAs(selectedBackUp.BackUpPath, ofd.FileName))
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "BackUp save as was completed successfully!");
            else
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.BackUp_View, "BackUp save as was not completed successfully!");
        }

        private void MenuItem_BackUpDelete_Click(object sender, RoutedEventArgs e)
        {
            BackUpInfoModel selectedBackUp = (BackUpInfoModel)Dg_BackUps.SelectedItem;
            BackUpSystem buSys = new BackUpSystem();

            if (selectedBackUp is null)
            {
                LoggerSystem.Log(LogStateEnum.Warning, LogPrefixEnum.BackUp_View, "BackUp delete was requested , but backUp was null!");
                return;
            }

            LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, $"BackUp delete was requested  {selectedBackUp.BackUpName}");

            if (buSys.Delete(selectedBackUp.BackUpPath))
            {
                Dg_BackUps.Items.Remove(selectedBackUp);
                //update counter
                MsgBox_BackUpCounter.Content = $"{Application.Current.Resources["backUpCount"] as string}: {Dg_BackUps.Items.Count}";
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.BackUp_View, "BackUp was deleted");
            }
            else
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.BackUp_View, "BackUp was NOT deleted");
        }


        public void ClearInfoProgressBar()
        {
            try
            {
                PB_InfoProgressBar.Dispatcher.Invoke(new Action(() =>
                { PB_InfoProgressBar.Value = 0; }));
            }
            catch { }
        }

        public void SetInfoProgressBarValue(int value)
        {
            try
            {
                PB_InfoProgressBar.Dispatcher.Invoke(new Action(() =>
                { PB_InfoProgressBar.Value += value; }));
            }
            catch { }
        }

        public void SetInfoProgressMaxValue(int value)
        {
            try
            {
                PB_InfoProgressBar.Dispatcher.Invoke(new Action(() =>
                { PB_InfoProgressBar.Maximum = value; }));
            }
            catch { }
        }
    }
}
