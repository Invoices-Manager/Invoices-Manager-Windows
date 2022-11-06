using System;
using System.Windows;

namespace InvoicesManager.Models
{
    public class InvoiceModel
    {
        public bool ShouldSerializeOpenInvoiceText(){return false;}
        public bool ShouldSerializeStringExhibitionDate(){return false;}

        public string OpenInvoiceText { get; } = Application.Current.Resources["open"] as string;
        public DateTime ExhibitionDate { get; set; }
        public string StringExhibitionDate { get { return ExhibitionDate.ToString("yyyy.MM.dd"); } }
        public string Organization { get; set; }
        public string DocumentType { get; set; }
        public string InvoiceNumber { get; set; }
        public string Reference { get; set; }
        public string Path { get; set; }
    }
}


/*
FileID
CaptureDate
ExhibitionDate
Reference
InvoiceNumber
DocumentType
Organization
Tags
MoneyState { Paid , Received,  NoInvoice }
MoneyTotal

 */
