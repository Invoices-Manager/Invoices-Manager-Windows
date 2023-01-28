namespace InvoicesManager.Views
{
    public partial class AboutWindow : Page
    {
        public AboutWindow()
            => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => Msg_PVersion.Content = $"Version: {Classes.EnvironmentsVariable.PROGRAM_VERSION}";
    }
}
