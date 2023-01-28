namespace InvoicesManager.Windows
{
    public partial class BackUpWindow : Page
    {
        public BackUpWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            EnvironmentsVariable.Window_BackUp_IsClosed = true;
        }

        private async void Bttn_BackUpCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog()
            {
                Filter = "BackUp-Datei (*.bkup)|*.bkup",
                RestoreDirectory = true
            };

            if (sfg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await Task.Run(() =>
                {
                    LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.BackUp_View, "BackUp was requested");
                    BackUpSystem buSys = new BackUpSystem();
                    if (buSys.BackUp(sfg.FileName))
                    {
                        LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.BackUp_View, "BackUp was completed successfully!");
                        MessageBox.Show(Application.Current.Resources["backUpSuccessfully"] as string);
                    }
                    else
                    {
                        LoggerSystem.Log(Classes.Enums.LogStateEnum.Warning, Classes.Enums.LogPrefixEnum.BackUp_View, "BackUp was not completed successfully!");
                        MessageBox.Show(Application.Current.Resources["backUpFailed"] as string);
                    }
                });
            }
        }

        private async void Bttn_BackUpRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "BackUp-Datei (*.bkup)|*.bkup"
            };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await Task.Run(() =>
                {
                    LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.BackUp_View, "Restore was requested");
                    BackUpSystem buSys = new BackUpSystem();
                    if (buSys.Restore(ofd.FileName))
                    {
                        LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.BackUp_View, "Restore was completed successfully!");
                    }
                    else
                    {
                        LoggerSystem.Log(Classes.Enums.LogStateEnum.Warning, Classes.Enums.LogPrefixEnum.BackUp_View, "Restore was not completed successfully!");
                        MessageBox.Show(Application.Current.Resources["backUpFailedRestored"] as string);
                    }
                });
            }
        }
    }
}
