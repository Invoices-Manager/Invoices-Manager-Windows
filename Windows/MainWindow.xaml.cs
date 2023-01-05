using InvoicesManager.Classes;
using InvoicesManager.Core;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace InvoicesManager.Windows
{
    public partial class MainWindow : Window
    {
                InvoiceMainWindow invoiceMainWindow;
        NotebookWindow notebookWindow;
        SettingWindow settingWindow;
        AboutWindow aboutWindow;
        
        public MainWindow()
        {
            InitializeComponent();
            //scan windows theme and set the app theme
            InitWindowsTheme();
            //load the settings
            ConfigSystem.Init();
            //init work path
            EnvironmentsVariable.InitWorkPath();
            //load the window UI language
            LanguageManager.Init();
            //init notebooks
            NotebookSystem.Init();
        }

        private void InitWindowsTheme()
        {
            //read the registry key
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            //get the value
            object value = key.GetValue("SystemUsesLightTheme");
            //set the theme
            if (Convert.ToInt32(value) == 0)
                EnvironmentsVariable.REGSystemUsesLightTheme = 0;
            else
                EnvironmentsVariable.REGSystemUsesLightTheme = 1;
        }
        
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
