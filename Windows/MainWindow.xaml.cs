using InvoicesManager.Classes;
using System.Windows;

namespace InvoicesManager.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
            => InitializeComponent();

        InvoiceMainWindow invoiceMainWindow;
        NotebookWindow notebookWindow;
        SettingWindow settingWindow;
        AboutWindow aboutWindow;

        private void Bttn_Open_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Will be added in a future version!");
            return;
        }
        
        private void Bttn_Open_Invoices_Click(object sender, RoutedEventArgs e)
        {
            if (EnvironmentsVariable.Window_Invoice_IsClosed)
            {
                invoiceMainWindow = new InvoiceMainWindow();
                EnvironmentsVariable.Window_Invoice_IsClosed = false;
            }

            invoiceMainWindow.Show();
            invoiceMainWindow.WindowState = WindowState.Normal;
            invoiceMainWindow.Focus();
        }
        
        private void Bttn_Open_Notebook_Click(object sender, RoutedEventArgs e)
        {
            if (EnvironmentsVariable.Window_Notebook_IsClosed)
            {
                notebookWindow = new NotebookWindow();
                EnvironmentsVariable.Window_Notebook_IsClosed = false;
            }

            notebookWindow.Show();
            notebookWindow.WindowState = WindowState.Normal;
            notebookWindow.Focus();
        }
        
        private void Bttn_Open_Setting_Click(object sender, RoutedEventArgs e)
        {
            if (EnvironmentsVariable.Window_Setting_IsClosed)
            {
                settingWindow = new SettingWindow();
                EnvironmentsVariable.Window_Setting_IsClosed = false;
            }

            settingWindow.Show();
            settingWindow.WindowState = WindowState.Normal;
            settingWindow.Focus();
        }
        
        private void Bttn_Open_About_Click(object sender, RoutedEventArgs e)
        {
            if (EnvironmentsVariable.Window_About_IsClosed)
            {
                aboutWindow = new AboutWindow();
                EnvironmentsVariable.Window_About_IsClosed = false;
            }

            aboutWindow.Show();
            aboutWindow.WindowState = WindowState.Normal;
            aboutWindow.Focus();
        }
    }
}
