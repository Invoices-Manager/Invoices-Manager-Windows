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
                //init the pages
                invoiceMainWindow = new InvoiceMainWindow();
                settingWindow = new SettingWindow();
                aboutWindow = new AboutWindow();
                backUpWindow = new BackUpWindow();
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
            => ViewMirror.Content = invoiceMainWindow;

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
            => ViewMirror.Content = backUpWindow;

        private void Bttn_Open_Setting_Click(object sender, RoutedEventArgs e)
            => ViewMirror.Content = settingWindow;

        private void Bttn_Open_About_Click(object sender, RoutedEventArgs e)
            => ViewMirror.Content = aboutWindow;

        private void Window_Closed(object sender, EventArgs e)
        {
            //kill the child windows, if the main window is closed
            if (!EnvironmentsVariable.Window_Notebook_IsClosed)
                notebookWindow.Close();
        }

        private void Bttn_SideBarSwapper_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_SideBar.Width == 280)
            {
                Animation_SideBar(280, 0);
                Animation_Frame(0, 70);

                Bttn_SideBarSwapper_Inner.Visibility = Visibility.Hidden;
                Bttn_SideBarSwapper_Outter.Visibility = Visibility.Visible;
            }
            else
            {
                Animation_SideBar(0, 280);
                Animation_Frame(70, 0);

                Bttn_SideBarSwapper_Inner.Visibility = Visibility.Visible;
                Bttn_SideBarSwapper_Outter.Visibility = Visibility.Hidden;
            }


        }

        private void Animation_SideBar(int from, int to)
        {
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

        private void Animation_Frame(int from, int to)
        {
            ThicknessAnimation animation = new ThicknessAnimation();
            animation.From = new Thickness(0, from, 0,0);
            animation.To = new Thickness(0, to, 0, 0);
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            animation.AccelerationRatio = 0.5;
            animation.DecelerationRatio = 0.5;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            Storyboard.SetTarget(animation, ViewMirror);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }
}
