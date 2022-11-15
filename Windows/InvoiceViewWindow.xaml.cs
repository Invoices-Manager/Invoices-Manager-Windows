using InvoicesManager.Classes;
using InvoicesManager.Classes.Enums;
using InvoicesManager.Core;
using InvoicesManager.Models;
using Microsoft.Win32;
using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace InvoicesManager.Windows
{
    public partial class InvoiceViewWindow : Window
    {
        private string filePath = String.Empty;
        private InvoiceViewModeEnum invoiceViewModeEnum;
        private InvoiceModel invoice;

        public InvoiceViewWindow(InvoiceViewModeEnum invoiceViewModeEnum, InvoiceModel invoice)
        {
            InitializeComponent();
            
            this.invoiceViewModeEnum = invoiceViewModeEnum;
            this.invoice = invoice;

            LoadInvoiceViewWindow();

            //The date of the exhibition is now by default today's date
            Dp_ExhibitionDate.SelectedDate = DateTime.Now;

            LoadTheEnumComBoxes();
            //Set the default value of the comboboxes
            Comb_ImportanceState.SelectedIndex = 2;
            Comb_MoneyState.SelectedIndex = 2;
            Comb_PaidState.SelectedIndex = 2;
        }

        private void LoadInvoiceViewWindow()
        {
            switch (invoiceViewModeEnum)
            {
                case InvoiceViewModeEnum.InvoiceAdd:
                    Title = $"{Application.Current.Resources["invoice"] as string} {Application.Current.Resources["add"] as string} | Made by Schecher | https://github.com/Schecher1";
                    Bttn_InvoiceAction.Content = Application.Current.Resources["addInvoice"] as string;
                    break;

                case InvoiceViewModeEnum.InvoiceEdit:
                    Title = $"{Application.Current.Resources["invoice"] as string} {Application.Current.Resources["edit"] as string} | Made by Schecher | https://github.com/Schecher1";
                    Bttn_InvoiceAction.Content = Application.Current.Resources["editInvoice"] as string;
                    Msg_file.Visibility = Visibility.Hidden;
                    Bttn_InvoiceFileAdd.Visibility = Visibility.Hidden;
                    LoadInvoiceData();
                    break;

                case InvoiceViewModeEnum.InvoiceDelete:
                    Title = $"{Application.Current.Resources["invoice"] as string} {Application.Current.Resources["delete"] as string} | Made by Schecher | https://github.com/Schecher1";
                    Bttn_InvoiceAction.Content = Application.Current.Resources["removeInvoice"] as string;
                    Msg_file.Visibility = Visibility.Hidden;
                    Bttn_InvoiceFileAdd.Visibility = Visibility.Hidden;
                    DoEverythingDisable();
                    LoadInvoiceData();
                    break;
            }
        }

        private void LoadTheEnumComBoxes()
        {
            Comb_ImportanceState.Items.Clear();
            Comb_MoneyState.Items.Clear();
            Comb_PaidState.Items.Clear();

            ComboBox[] comboBoxes = new ComboBox[] { Comb_ImportanceState, Comb_MoneyState, Comb_PaidState };
            String[] keyWords = new String[] { "veryImportant", "important", "neutral", "unimportant", "paid", "received", "noInvoice", "paid", "unpaid", "noInvoice" };

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

        private void DoEverythingDisable()
        {
            Dp_ExhibitionDate.IsEnabled = false;
            Tb_Organization.IsEnabled = false;
            Tb_DocumentType.IsEnabled = false;
            Tb_InvoiceNumber.IsEnabled = false;
            Tb_Reference.IsEnabled = false;
        }

        private void LoadInvoiceData()
        {
            Dp_ExhibitionDate.SelectedDate = invoice.ExhibitionDate;
            Tb_Organization.Text = invoice.Organization;
            Tb_DocumentType.Text = invoice.DocumentType;
            Tb_InvoiceNumber.Text = invoice.InvoiceNumber;
            Tb_Reference.Text = invoice.Reference;
            Tb_MoneyTotal.Text = invoice.MoneyTotal.ToString();
            Tb_Tags.Text = String.Join(";", invoice.Tags);
            Comb_ImportanceState.SelectedIndex = (int)invoice.ImportanceState;
            Comb_MoneyState.SelectedIndex = (int)invoice.MoneyState;
            Comb_PaidState.SelectedIndex = (int)invoice.PaidState;
        }

        private bool CheckIfAllIsValide()
        {
            if (String.IsNullOrWhiteSpace(Tb_Reference.Text))
                return false;
            if (String.IsNullOrWhiteSpace(Tb_InvoiceNumber.Text))
                return false;
            if (String.IsNullOrWhiteSpace(Tb_Organization.Text))
                return false;
            if (String.IsNullOrWhiteSpace(Tb_DocumentType.Text))
                return false;
            if (Dp_ExhibitionDate.SelectedDate == default)
                return false;
            if (filePath == String.Empty && invoiceViewModeEnum == InvoiceViewModeEnum.InvoiceAdd)
                return false;

            return true;
        }

        private void ClearAllInputs()
        {
            Tb_FilePath.Text = String.Empty;
            Tb_Organization.Text = String.Empty;
            Tb_Reference.Text = String.Empty;
            Tb_InvoiceNumber.Text = String.Empty;
            Tb_DocumentType.Text = String.Empty;
            Dp_ExhibitionDate.SelectedDate = default;
            filePath = String.Empty;
        }

        private void Bttn_InvoiceAction_Click(object sender, RoutedEventArgs e)
        {
            switch (invoiceViewModeEnum)
            {
                case InvoiceViewModeEnum.InvoiceAdd:
                    Bttn_InvoiceAdd_Click();
                    break;

                case InvoiceViewModeEnum.InvoiceEdit:
                    Bttn_InvoiceEdit_Click();
                    break;

                case InvoiceViewModeEnum.InvoiceDelete:
                    Bttn_InvoiceDelete_Click();
                    break;
            }
        }


        //DELETE AREA START
        private void Bttn_InvoiceDelete_Click()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this invoice?", "Remove Invoice", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                InvoiceSystem.RemoveInvoice(invoice);
                InvoiceSystem.Init();
            }
            Close();
        }
        //DELETE AREA END


        //EDIT AREA START
        private void Bttn_InvoiceEdit_Click()
        {
            if (!CheckIfAllIsValide())
            {
                MessageBox.Show("Please Check you data input", "Error", MessageBoxButton.OK);
                return;
            }

            InvoiceModel editInvoice = new InvoiceModel()
            {
                //Path = Tb_FilePath.Text,
                Organization = Tb_Organization.Text,
                Reference = Tb_Reference.Text,
                InvoiceNumber = Tb_InvoiceNumber.Text,
                DocumentType = Tb_DocumentType.Text,
                ExhibitionDate = Dp_ExhibitionDate.SelectedDate.Value
            };

            InvoiceSystem.EditInvoice(invoice, editInvoice);
            InvoiceSystem.Init();

            Close();
        }
        //EDIT AREA END

        
        //ADD AREA START
        private void Bttn_InvoiceAdd_Click()
        {
            if (!CheckIfAllIsValide())
            {
                MessageBox.Show(Application.Current.Resources["checkYouInput"] as string, Application.Current.Resources["error"] as string, MessageBoxButton.OK);
                return;
            }

            AddNewInvoice();
            ClearAllInputs();
        }
        private void Bttn_InvoiceFileAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
                Tb_FilePath.Text = filePath;
            }
        }
        private void AddNewInvoice()
        {
            string hashID = HashManager.GetMD5HashFromFile(filePath);
            string newPath = @$"{EnvironmentsVariable.PathInvoices}{hashID}.pdf";

            InvoiceModel newInvoice = new InvoiceModel()
            {
                InvoiceNumber = Tb_InvoiceNumber.Text,
                Reference = Tb_Reference.Text,
                Organization = Tb_Organization.Text,
                DocumentType = Tb_DocumentType.Text,
                ExhibitionDate = Dp_ExhibitionDate.SelectedDate.Value,
                //Path = newPath
            };

            InvoiceSystem.AddInvoice(newInvoice, filePath, newPath);
            InvoiceSystem.Init();
        }
        //ADD AREA END
    }
}
