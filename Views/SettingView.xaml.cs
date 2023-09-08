﻿namespace InvoicesManager.Views
{
    public partial class SettingView : Page
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Comb_UILanguage.Items.Clear();
                Comb_MoneyUnit.Items.Clear();

                foreach (var uiL in EnvironmentsVariable.PossibleUILanguages)
                    Comb_UILanguage.Items.Add(uiL);

                foreach (var mU in EnvironmentsVariable.PossibleMoneyUnits)
                    Comb_MoneyUnit.Items.Add(mU);

                Tb_PDFProgramPath.Text = EnvironmentsVariable.PathPDFBrowser;
                Comb_UILanguage.Text = EnvironmentsVariable.UILanguage;
                Comb_MoneyUnit.Text = EnvironmentsVariable.MoneyUnit.ToString();
                Tb_LogPath.Text = EnvironmentsVariable.PathLogs;
                //Cb_EveryStartUpBackUp.IsChecked = EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts;
                //Tb_MaxCountBackUp.Text = EnvironmentsVariable.MaxCountBackUp.ToString();
                Tb_HostProt.Text = EnvironmentsVariable.HOST_PROT;
                Tb_HostAddress.Text = EnvironmentsVariable.HOST_ADDRESS;
                Tb_HostPort.Text = EnvironmentsVariable.HOST_PORT;
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Setting_View, "Error while loading settings, err: " + ex.Message);
            }
        }

        private void Bttn_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Comb_UILanguage.SelectedIndex == -1 || String.IsNullOrWhiteSpace(Tb_PDFProgramPath.Text))
                {
                    MessageBox.Show(Application.Current.Resources["checkYouInput"] as string, Application.Current.Resources["error"] as string);
                    return;
                }

                EnvironmentsVariable.PathPDFBrowser = Tb_PDFProgramPath.Text;
                EnvironmentsVariable.UILanguage = Comb_UILanguage.Text;
                EnvironmentsVariable.MoneyUnit = Convert.ToChar(Comb_MoneyUnit.Text);
                EnvironmentsVariable.PathLogs = Tb_LogPath.Text;
                //EnvironmentsVariable.CreateABackupEveryTimeTheProgramStarts = Cb_EveryStartUpBackUp.IsChecked.Value;
                //EnvironmentsVariable.MaxCountBackUp = Convert.ToInt32(Tb_MaxCountBackUp.Text);
                EnvironmentsVariable.HOST_PROT = Tb_HostProt.Text;
                EnvironmentsVariable.HOST_ADDRESS = Tb_HostAddress.Text;
                EnvironmentsVariable.HOST_PORT = Tb_HostPort.Text;

                EnvironmentsVariable.InitWorkPath();
                ConfigSystem cSys = new ConfigSystem();
                cSys.SaveIntoJsonFile();
                LanguageManager.Init();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Setting_View, "Error while saving settings, err: " + ex.Message);
            }
        }

        //private void Tb_MaxCountBackUp_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    //check if its a number
        //    //check if its not above int32 max value
        //    //check if its not below 1
        //    try
        //    {
        //        Convert.ToInt32(Tb_MaxCountBackUp.Text);

        //      if (Convert.ToInt32(Tb_MaxCountBackUp.Text) < 1)
        //            Tb_MaxCountBackUp.Text = "1";
        //    }
        //    catch (System.OverflowException)
        //    {
        //        Tb_MaxCountBackUp.Text = Int32.MaxValue.ToString();
        //    }
        //    catch (System.FormatException)
        //    {
        //        Tb_MaxCountBackUp.Text = "1";
        //    }
        //}

        private void Bttn_Select_PDFProgramPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "(*.exe)|*.exe";

            if (fd.ShowDialog() == DialogResult.OK)
                Tb_PDFProgramPath.Text = fd.FileName;
        }

        private void Bttn_Select_LogPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
                Tb_LogPath.Text = fbd.SelectedPath + "\\";
        }

        private void Bttn_OpenTemplateMgr_Click(object sender, RoutedEventArgs e)
        {
            InvoiceTemplateWindow invoiceTemplateWindow = new InvoiceTemplateWindow();
            invoiceTemplateWindow.ShowDialog();
        }
    }
}
