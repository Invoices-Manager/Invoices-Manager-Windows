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
        public InvoiceModel Invoice { get; set; }
    }
}
