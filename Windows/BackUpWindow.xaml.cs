using InvoicesManager.Classes;
using InvoicesManager.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace InvoicesManager.Windows
{
    public partial class BackUpWindow : Window
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
                    BackUpSystem buSys = new BackUpSystem();
                    if (buSys.BackUp(sfg.FileName))
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

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await Task.Run(() =>
                {
                    BackUpSystem buSys = new BackUpSystem();
                    if (!buSys.Restore(ofd.FileName))
                        MessageBox.Show(this.Resources["backUpFailedRestored"] as string);
                });
            }
        }
    }
}
