﻿namespace InvoicesManager.Windows
{
    public partial class InvoiceSaveAsWindow : Window
    {
        readonly InvoiceModel invoice;
        private string fileName = string.Empty;

        //ORGA_INCNR_DATE => Rb_Vers01
        //DATE_ORGA_INCNR => Rb_Vers02
        //INCNR_ORGA_DATE => Rb_Vers03
        //DATE_ORGA_INCNR => Rb_Vers04
 
        public InvoiceSaveAsWindow(InvoiceModel invoice)
        {
            InitializeComponent();
            this.invoice = invoice;
            LoadRadioButtonPreview();
            Tb_CustomName.Visibility = Visibility.Hidden;
        }

        private void LoadRadioButtonPreview()
        {
            Rb_Vers01.Content = $"{invoice.Organization}-{invoice.InvoiceNumber}-{invoice.ExhibitionDate.ToString("yyyyMMdd")}.pdf";
            Rb_Vers02.Content = $"{invoice.ExhibitionDate.ToString("yyyyMMdd")}-{invoice.Organization}-{invoice.InvoiceNumber}.pdf";
            Rb_Vers03.Content = $"{invoice.InvoiceNumber}-{invoice.Organization}-{invoice.ExhibitionDate.ToString("yyyyMMdd")}.pdf";
            Rb_Vers04.Content = $"{invoice.ExhibitionDate.ToString("yyyyMMdd")}-{invoice.Organization}-{invoice.InvoiceNumber}.pdf";
        }

        private void Rb_Vers_Click(object sender, RoutedEventArgs e)
        {
            RadioButton RbSender = (RadioButton)sender;

            switch (RbSender.Name)
            {
                case "Rb_Vers01":
                    Tb_CustomName.Visibility = Visibility.Hidden;
                    fileName = $"{invoice.Organization}-{invoice.InvoiceNumber}-{invoice.ExhibitionDate.ToString("yyyyMMdd")}.pdf";
                    break;
                case "Rb_Vers02":
                    Tb_CustomName.Visibility = Visibility.Hidden;
                    fileName = $"{invoice.ExhibitionDate.ToString("yyyyMMdd")}-{invoice.Organization}-{invoice.InvoiceNumber}.pdf";
                    break;
                case "Rb_Vers03":
                    Tb_CustomName.Visibility = Visibility.Hidden;
                    fileName = $"{invoice.InvoiceNumber}-{invoice.Organization}-{invoice.ExhibitionDate.ToString("yyyyMMdd")}.pdf";
                    break;
                case "Rb_Vers04":
                    Tb_CustomName.Visibility = Visibility.Hidden;
                    fileName = $"{invoice.ExhibitionDate.ToString("yyyyMMdd")}-{invoice.Organization}-{invoice.InvoiceNumber}.pdf";
                    break;
                case "Rb_Custom":
                    Tb_CustomName.Visibility = Visibility.Visible;
                    fileName = $" ";
                    break;
            }
        }

        private void Bttn_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            // fileName == empty & Tb_CustomName.Text != string.Empty&& Rb_Custom.IsChecked = true   => Custom name

            if (fileName == string.Empty ||  String.IsNullOrWhiteSpace(Tb_CustomName.Text) && Rb_Custom.IsChecked == true)
            {
                MessageBox.Show("Please select a file name version or write a custom name");
                return;
            }

            if (Rb_Custom.IsChecked == true)
                fileName = Tb_CustomName.Text + ".pdf";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Invoice";
            sfd.CheckPathExists = true;
            sfd.FileName = fileName;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.Filter = "PDF files (*.pdf)|*.txt|All files (*.*)|*.*";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InvoiceSystem iSys = new InvoiceSystem();
                iSys.SaveAs(invoice, sfd.FileName);
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.SaveAs_View, $"Invoice was saved as {fileName} in {sfd.FileName}");
                this.Close();
            }

        }
    }
}
