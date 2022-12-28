using InvoicesManager.Classes;
using InvoicesManager.Core;
using System;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

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

            Tb_PDFProgramPath.Text = EnvironmentsVariable.PathPDFBrowser;
            Comb_UILanguage.Text = EnvironmentsVariable.UILanguage;
            Tb_InvoicePath.Text = EnvironmentsVariable.PathInvoices;
            Tb_NotebookPath.Text = EnvironmentsVariable.PathNotebook;
            Tb_BackUpPath.Text = EnvironmentsVariable.PathBackUps;
            Cb_EveryStartUpBackUp.IsChecked = EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts;
            Tb_MaxCountBackUp.Text = EnvironmentsVariable.MaxCountBackUp.ToString();
        }

        private void Bttn_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (Comb_UILanguage.SelectedIndex == -1 || String.IsNullOrWhiteSpace(Tb_PDFProgramPath.Text))
            {
                MessageBox.Show(Application.Current.Resources["checkYouInput"] as string, Application.Current.Resources["error"] as string);
                return;
            }

            EnvironmentsVariable.PathPDFBrowser = Tb_PDFProgramPath.Text;
            EnvironmentsVariable.UILanguage = Comb_UILanguage.Text;
            EnvironmentsVariable.PathInvoices = Tb_InvoicePath.Text;
            EnvironmentsVariable.PathNotebook = Tb_NotebookPath.Text;
            EnvironmentsVariable.PathBackUps = Tb_BackUpPath.Text;
            EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts = Cb_EveryStartUpBackUp.IsChecked.Value;
            EnvironmentsVariable.MaxCountBackUp = Convert.ToInt32(Tb_MaxCountBackUp.Text);

            EnvironmentsVariable.InitWorkPath();
            ConfigSystem.Save();
            LanguageManager.Init();
            NotebookSystem.Init();
        }

        private void Tb_MaxCountBackUp_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //check if its a number
            //check if its not above int32 max value
            //check if its not below 1
            try
            {
                Convert.ToInt32(Tb_MaxCountBackUp.Text);

              if (Convert.ToInt32(Tb_MaxCountBackUp.Text) < 1)
                    Tb_MaxCountBackUp.Text = "1";
            }
            catch (System.OverflowException)
            {
                Tb_MaxCountBackUp.Text = Int32.MaxValue.ToString();
            }
            catch (System.FormatException)
            {
                Tb_MaxCountBackUp.Text = "1";
            }
        }

        private void Bttn_Select_BackUpPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Tb_BackUpPath.Text = fbd.SelectedPath + "\\";
        }

        private void Bttn_Select_InvoicePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Tb_InvoicePath.Text = fbd.SelectedPath + "\\";
        }

        private void Bttn_Select_PDFProgramPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "(*.exe)|*.exe";

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Tb_PDFProgramPath.Text = fd.FileName;
        }

        private void Bttn_Select_NotebookPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Tb_NotebookPath.Text = fbd.SelectedPath + "\\";
        }
    }
}
