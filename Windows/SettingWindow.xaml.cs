using InvoicesManager.Classes;
using InvoicesManager.Core;
using System;
using System.Windows;

namespace InvoicesManager.Windows
{
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Comb_UILanguage.Items.Clear();
            
            foreach (string uiL in EnvironmentsVariable.UILanguages)
                Comb_UILanguage.Items.Add(uiL);

            Tb_PathData.Text = EnvironmentsVariable.PathData;
            Tb_PathProgram.Text = EnvironmentsVariable.PathPDFBrowser;
        }

        private void Bttn_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (Comb_UILanguage.SelectedIndex == -1 || String.IsNullOrWhiteSpace(Tb_PathProgram.Text) || String.IsNullOrWhiteSpace(Tb_PathData.Text))
            {
                MessageBox.Show(Application.Current.Resources["checkYouInput"] as string, Application.Current.Resources["error"] as string);
                return;
            }

            EnvironmentsVariable.PathData = Tb_PathData.Text;
            EnvironmentsVariable.PathPDFBrowser = Tb_PathProgram.Text;
            EnvironmentsVariable.UILanguage = Comb_UILanguage.Text;

            SettingSystem.Save();
        }
    }
}
