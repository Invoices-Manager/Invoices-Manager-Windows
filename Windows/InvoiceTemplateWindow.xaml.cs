namespace InvoicesManager.Windows
{
    public partial class InvoiceTemplateWindow : Window
    {
        private InvoiceModel invoice;
        private TemplateModel templateCache;

        public InvoiceTemplateWindow()
        {
            InitializeComponent();
            
            LoadTheEnumComBoxes();
            //Set the default value of the comboboxes
            Comb_ImportanceState.SelectedIndex = 2;
            Comb_MoneyState.SelectedIndex = 2;
            Comb_PaidState.SelectedIndex = 2;

            //init the templates
            TemplateSystem _ts = new TemplateSystem();
            _ts.Init();
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

        private void Bttn_Create_Click(object sender, RoutedEventArgs e)
        {
            //check if the template name is empty
            if (Tb_TemplateName.Text == String.Empty)
            {
                MessageBox.Show(Application.Current.Resources["templateNameEmpty"] as string, Application.Current.Resources["error"] as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            //check if the template is already used
            if (EnvironmentsVariable.AllTemplates.Any(x => x.Name == Tb_TemplateName.Text))
            {
                MessageBox.Show(Application.Current.Resources["templateAlreadyExist"] as string, Application.Current.Resources["error"] as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //get a new template model
            TemplateModel newTemplate = GetDataAsTemplate();

            //add the template
            TemplateSystem _ts = new TemplateSystem();
            _ts.AddTemplate(newTemplate);

            //init, clear the inputs and refresh the combobox
            _ts.Init();
            ClearAllInputs();
            RefreshTemplates();
            Tb_TemplateName.Text = String.Empty;
        }

        private void Bttn_Save_Click(object sender, RoutedEventArgs e)
        {
            //check if a template is selected
            if (Comb_Templates.SelectedIndex == -1)
            {
                MessageBox.Show(Application.Current.Resources["templateNotSelected"] as string, Application.Current.Resources["error"] as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //get a new template model
            TemplateModel newTemplate = GetDataAsTemplate();
            newTemplate.Name = Tb_TemplateName.Text;

            //save the template
            TemplateSystem _ts = new TemplateSystem();
            _ts.EditTemplate(templateCache.Name, newTemplate);

            //init, clear the inputs and refresh the combobox
            _ts.Init();
            ClearAllInputs();
            RefreshTemplates();
            Tb_TemplateName.Text = String.Empty;
        }

        private void Bttn_Delete_Click(object sender, RoutedEventArgs e)
        {
            //check if a template is selected
            if (Comb_Templates.SelectedIndex == -1)
            {
                MessageBox.Show(Application.Current.Resources["templateNotSelected"] as string, Application.Current.Resources["error"] as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //delete the template
            TemplateSystem _ts = new TemplateSystem();
            //get template
            TemplateModel template = EnvironmentsVariable.AllTemplates.Find(x => x.Name == Comb_Templates.Text);

            //make sure the user want to delete the template
            MessageBoxResult result = MessageBox.Show(Application.Current.Resources["deleteTemplateMsg"] as string, Application.Current.Resources["deleteTemplateTitle"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                _ts.RemoveTemplate(template);

            //init, clear the inputs and refresh the combobox
            _ts.Init();
            ClearAllInputs();
            RefreshTemplates();
            Tb_TemplateName.Text = String.Empty;
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
        
        private void RefreshTemplates()
        {
            Comb_Templates.Items.Clear();
            foreach (var template in EnvironmentsVariable.AllTemplates)
                Comb_Templates.Items.Add(template.Name);
        }

        private TemplateModel GetDataAsTemplate()
        {
            //create a new template model
            TemplateModel newTemplate = new TemplateModel()
            {
                Name = Tb_TemplateName.Text,
                Template = new InvoiceModel()
                {
                    FileID = "",
                    Organization = Tb_Organization.Text,
                    Reference = Tb_Reference.Text,
                    InvoiceNumber = Tb_InvoiceNumber.Text,
                    DocumentType = Tb_DocumentType.Text,
                    MoneyTotal = Tb_MoneyTotal.Text == String.Empty ? -1 : Convert.ToDouble(Tb_MoneyTotal.Text),
                    Tags = Tb_Tags.Text.Split(';'),
                    ImportanceState = ImportanceState.StringAsEnum(Comb_ImportanceState.SelectedItem.ToString()),
                    MoneyState = MoneyState.StringAsEnum(Comb_MoneyState.SelectedItem.ToString()),
                    PaidState = PaidState.StringAsEnum(Comb_PaidState.SelectedItem.ToString()),
                }
            };

            return newTemplate;
        }

        private void Comb_Templates_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //return if null
            if (Comb_Templates.SelectedIndex == -1)
                return;

            //get the template
            TemplateModel template = EnvironmentsVariable.AllTemplates.Find(x => x.Name == Comb_Templates.SelectedItem.ToString());

            //check if the template is null
            if (template == null)
                return;

            //load the template data
            invoice = template.Template;
            templateCache = GetDataAsTemplate();
            templateCache.Name = template.Name;
            Tb_TemplateName.Text = template.Name;
            LoadInvoiceData();
        }

        private void Tb_MoneyTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            //check if the text is a number, if not clear it
            if (!double.TryParse(Tb_MoneyTotal.Text, out double result))
                Tb_MoneyTotal.Text = String.Empty;

            //check if the number is over int max or below int min
            if (result > int.MaxValue || result < int.MinValue)
                Tb_MoneyTotal.Text = String.Empty;
        }
    }
}
