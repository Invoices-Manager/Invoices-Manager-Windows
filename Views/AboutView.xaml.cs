namespace InvoicesManager.Views
{
    public partial class AboutView : Page
    {
        public AboutView()
            => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => Msg_PVersion.Content = $"Version: {Classes.EnvironmentsVariable.PROGRAM_VERSION}";
    }
}