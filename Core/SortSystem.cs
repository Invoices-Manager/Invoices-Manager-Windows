using InvoicesManager.Classes;
using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoicesManager.Core
{
    public class SortSystem
    {
        List<InvoiceModel> allInvoices = new List<InvoiceModel>();
        
        private readonly string filterReference = String.Empty;
        private readonly string filterInvoiceNumber = String.Empty;
        private readonly string filterOrganization = "-1";
        private readonly string filterDocumentType = "-1";
        private readonly DateTime filterExhibitionDate = default;
        
        public SortSystem(List<InvoiceModel> allInvoices, string filterReference, string filterInvoiceNumber, string filterOrganization, string filterDocumentType, DateTime filterExhibitionDate)
        {
            this.allInvoices = allInvoices;
            this.filterReference = filterReference.ToLower();
            this.filterInvoiceNumber = filterInvoiceNumber.ToLower();
            this.filterOrganization = filterOrganization.ToLower();
            this.filterDocumentType = filterDocumentType.ToLower();
            this.filterExhibitionDate = filterExhibitionDate;
        }
        
        public void Sort()
        {
            EnvironmentsVariable.FilteredInvoices = allInvoices
                .Where(x => x.Reference.ToLower().Contains(filterReference) || String.IsNullOrEmpty(filterReference))
                .Where(x => x.InvoiceNumber.ToLower().Contains(filterInvoiceNumber) || String.IsNullOrEmpty(filterInvoiceNumber))
                .Where(x => x.Organization.ToLower().Equals(filterOrganization) || filterOrganization is "-1")
                .Where(x => x.DocumentType.ToLower().Equals(filterDocumentType) || filterDocumentType is "-1")
                .Where(x => x.ExhibitionDate.Date == filterExhibitionDate.Date || filterExhibitionDate == default)
                .ToList();
        }
    }
}
