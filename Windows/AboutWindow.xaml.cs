using InvoicesManager.Classes;
using System;
using System.Windows;

namespace InvoicesManager.Windows
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
            => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => Msg_PVersion.Content = $"Version: {Classes.EnvironmentsVariable.PROGRAM_VERSION}";

        private void Window_Closed(object sender, EventArgs e)
            => EnvironmentsVariable.Window_About_IsClosed = true;
    }
}
