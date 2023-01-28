using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace InvoicesManager.Windows
{
    public partial class MainWindow : Window
    {
        InvoiceMainWindow invoiceMainWindow;
        NotebookWindow notebookWindow;
        SettingWindow settingWindow;
        AboutWindow aboutWindow;
        BackUpWindow backUpWindow;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                //scan windows theme and set the app theme
                InitWindowsTheme();
                //init work path
                EnvironmentsVariable.InitWorkPath();
                //load the settings
                ConfigSystem cSys = new ConfigSystem();
                cSys.Init();
                //load the window UI language
                LanguageManager.Init();
                //init notebooks
                NotebookSystem nSys = new NotebookSystem();
                nSys.Init();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.MainWindow_View, ex.Message);
            }
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
        
        private void Bttn_Open_BackUp_Click(object sender, RoutedEventArgs e)
        {
            if (EnvironmentsVariable.Window_BackUp_IsClosed)
            {
                backUpWindow = new BackUpWindow();
                EnvironmentsVariable.Window_BackUp_IsClosed = false;
            }

            backUpWindow.Show();
            backUpWindow.WindowState = WindowState.Normal;
            backUpWindow.Focus();
        }

        private void Bttn_Open_Setting_Click(object sender, RoutedEventArgs e)
        {
            ViewMirror.Content = new SettingWindow();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            //kill the child windows, if the main window is closed
            if (!EnvironmentsVariable.Window_Invoice_IsClosed)
                invoiceMainWindow.Close();
            if (!EnvironmentsVariable.Window_Notebook_IsClosed)
                notebookWindow.Close();
            if (!EnvironmentsVariable.Window_BackUp_IsClosed)
                backUpWindow.Close();
            if (!EnvironmentsVariable.Window_About_IsClosed)
                aboutWindow.Close();
        }

        private void Bttn_SideBarSwapper_Click(object sender, RoutedEventArgs e)
        {
            int from, to;

            if (Grid_SideBar.Width == 280)
            {
                from = 280;
                to = 0;
                Bttn_SideBarSwapper_Inner.Visibility = Visibility.Hidden;
                Bttn_SideBarSwapper_Outter.Visibility = Visibility.Visible;
            }
            else
            {
                from = 0;
                to = 280;
                Bttn_SideBarSwapper_Inner.Visibility = Visibility.Visible;
                Bttn_SideBarSwapper_Outter.Visibility = Visibility.Hidden;
            }

            Storyboard storyboard = new Storyboard();
            storyboard.Duration = new Duration(TimeSpan.FromSeconds(1));

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.AccelerationRatio = 0.2;
            animation.DecelerationRatio = 0.8;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            Grid_SideBar.BeginStoryboard(storyboard);
        }
    }
}
