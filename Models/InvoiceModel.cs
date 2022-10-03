using System;

namespace InvoicesManager.Models
{
    public class InvoiceModel
    {
        public string OpenInvoiceText { get; } = "Öffnen";
        public DateTime ExhibitionDate { get; set; }
        public string Organization { get; set; }
        public string DocumentType { get; set; }
        public string InvoiceNumber { get; set; }
        public string Reference { get; set; }
        public string Path { get; set; }
    }
}
