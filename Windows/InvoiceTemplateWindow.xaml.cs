namespace InvoicesManager.Windows
{
    public partial class InvoiceTemplateWindow : Window
    {
        private InvoiceModel invoice;

        public InvoiceTemplateWindow()
        {
            InitializeComponent();
            
            //The date of the exhibition is now by default today's date
            Dp_ExhibitionDate.SelectedDate = DateTime.Now;

            LoadTheEnumComBoxes();
            //Set the default value of the comboboxes
            Comb_ImportanceState.SelectedIndex = 2;
            Comb_MoneyState.SelectedIndex = 2;
            Comb_PaidState.SelectedIndex = 2;

            //init the templates
            foreach (var template in EnvironmentsVariable.AllTemplates)
                Comb_Templates.Items.Add(template.Name);

            //set title
            Title = $"{Application.Current.Resources["templateMgr"] as string} | Made by Schecher | https://github.com/Schecher1";
        }

        private void LoadTheEnumComBoxes()
        {
            Comb_ImportanceState.Items.Clear();
            Comb_MoneyState.Items.Clear();
            Comb_PaidState.Items.Clear();

            ComboBox[] comboBoxes = new ComboBox[] { Comb_ImportanceState, Comb_MoneyState, Comb_PaidState };
            string[] keyWords = new string[] { "veryImportant", "important", "neutral", "unimportant", "paid", "received", "noInvoice", "paid", "unpaid", "noInvoice" };

            for (int i = 0; i <= 9; i++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        comboBoxes[0].Items.Add(Application.Current.Resources[keyWords[i]] as string);
                        break;

                    case 4:
                    case 5:
                    case 6:
                        comboBoxes[1].Items.Add(Application.Current.Resources[keyWords[i]] as string);
                        break;

                    case 7:
                    case 8:
                    case 9:
                        comboBoxes[2].Items.Add(Application.Current.Resources[keyWords[i]] as string);
                        break;
                }
            }
        }

        private void LoadInvoiceData()
        {
            Dp_ExhibitionDate.SelectedDate = invoice.ExhibitionDate;
            Tb_Organization.Text = invoice.Organization;
            Tb_DocumentType.Text = invoice.DocumentType;
            Tb_InvoiceNumber.Text = invoice.InvoiceNumber == "" ? "" : invoice.InvoiceNumber;
            Tb_Reference.Text = invoice.Reference;
            Tb_MoneyTotal.Text = invoice.MoneyTotal == -1 ? "" : invoice.MoneyTotal.ToString();
            Tb_Tags.Text = String.Join(";", invoice.Tags);
            Comb_ImportanceState.SelectedIndex = (int)invoice.ImportanceState;
            Comb_MoneyState.SelectedIndex = (int)invoice.MoneyState;
            Comb_PaidState.SelectedIndex = (int)invoice.PaidState;
        }

        private void ClearAllInputs()
        {
            Tb_Organization.Text = String.Empty;
            Tb_Reference.Text = String.Empty;
            Tb_InvoiceNumber.Text = String.Empty;
            Tb_DocumentType.Text = String.Empty;
            Tb_MoneyTotal.Text = String.Empty;
            Tb_Tags.Text = String.Empty;
            Comb_ImportanceState.SelectedIndex = 2;
            Comb_MoneyState.SelectedIndex = 2;
            Comb_PaidState.SelectedIndex = 2;
        }

        private void Bttn_Create_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
