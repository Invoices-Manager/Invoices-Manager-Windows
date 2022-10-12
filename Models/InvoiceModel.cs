using System;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;

namespace InvoicesManager.Models
{
    public class InvoiceModel
    {
        public bool ShouldSerializeOpenInvoiceText()
        {
            return false;
        }

        public string OpenInvoiceText { get; } = Application.Current.Resources["open"] as string;
        public DateTime ExhibitionDate { get; set; }
        public string Organization { get; set; }
        public string DocumentType { get; set; }
        public string InvoiceNumber { get; set; }
        public string Reference { get; set; }
        public string Path { get; set; }
    }
}
