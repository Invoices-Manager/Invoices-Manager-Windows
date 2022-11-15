using InvoicesManager.Classes.Enums;
using System;
using System.Collections.Generic;

namespace InvoicesManager.Models
{
    public class BackUpModel
    {
        public int EntityCount { get; set; }
        public DateTime DateOfCreation { get; set; }
        public List<InvoiceBackUpModel> Invoices { get; set; }
    }

    public class InvoiceBackUpModel
    {
        public string Base64 { get; set; }
        public SubInvoiceBackUpModel Invoice { get; set; }
    }

    public class SubInvoiceBackUpModel
    {
        public string FileID { get; set; } 
        public DateTime CaptureDate { get; set; } 
        public DateTime ExhibitionDate { get; set; }
        public string Reference { get; set; } 
        public string DocumentType { get; set; } 
        public string Organization { get; set; } 
        public string InvoiceNumber { get; set; }
        public string[] Tags { get; set; }
        public ImportanceStateEnum ImportanceState { get; set; } 
        public MoneyStateEnum MoneyState { get; set; }
        public PaidStateEnum PaidState { get; set; } 
        public double MoneyTotal { get; set; }
    }
}
