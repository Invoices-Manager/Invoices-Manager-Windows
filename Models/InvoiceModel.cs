using System;
using System.Windows;
using InvoicesManager.Classes.Enums;

namespace InvoicesManager.Models
{
    public class InvoiceModel
    {
        public bool ShouldSerializeOpenInvoiceText() { return false; }
        public bool ShouldSerializeStringExhibitionDate() { return false; }

        public string OpenInvoiceText { get; } = Application.Current.Resources["open"] as string;
           
        public string FileID { get; set; } // (NN)
        public DateTime CaptureDate { get; set; } // (NN)
        public DateTime ExhibitionDate { get; set; } // (NN)      
        public string StringExhibitionDate { get { return ExhibitionDate.ToString("yyyy.MM.dd"); } }
        public string Reference { get; set; } // (NN)
        public string DocumentType { get; set; } // (NN)
        public string Organization { get; set; } // (NN)
        public string InvoiceNumber { get; set; }
        public string[] Tags { get; set; }
        public ImportanceStateEnum ImportanceState { get; set; } // { VeryImportant, Important, Neutral, Unimportant }
        public MoneyStateEnum MoneyState { get; set; } // { Paid , Received,  NoInvoice }
        public PaidStateEnum PaidState { get; set; } // { Paid , Unpaid,  NoInvoice }
        public double MoneyTotal { get; set; }
    }
}

//OLD

//public class InvoiceModel
//{
//    public bool ShouldSerializeOpenInvoiceText() { return false; }
//    public bool ShouldSerializeStringExhibitionDate() { return false; }

//    public string OpenInvoiceText { get; } = Application.Current.Resources["open"] as string;
//    public DateTime ExhibitionDate { get; set; }
//    public string StringExhibitionDate { get { return ExhibitionDate.ToString("yyyy.MM.dd"); } }
//    public string Organization { get; set; }
//    public string DocumentType { get; set; }
//    public string InvoiceNumber { get; set; }
//    public string Reference { get; set; }
//    public string Path { get; set; }
//}