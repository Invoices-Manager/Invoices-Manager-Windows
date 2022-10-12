using InvoicesManager.Classes;
using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoicesManager.Core
{
    public class SortSystem
    {
        List<InvoiceModel> allInvoices = new List<InvoiceModel>();
        List<InvoiceModel> sortInvoices = new List<InvoiceModel>();

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
            
            foreach (var invoice in allInvoices)
            {
                if (!(invoice.Reference.ToLower().Contains(filterReference) || filterReference == String.Empty))
                    continue;
                if (!(invoice.InvoiceNumber.ToLower().Contains(filterInvoiceNumber) || filterInvoiceNumber == String.Empty))
                    continue;
                if (!(invoice.Organization.ToLower() == filterOrganization || filterOrganization == "-1"))
                    continue;
                if (!(invoice.DocumentType.ToLower() == filterDocumentType || filterDocumentType == "-1"))
                    continue;
                if (!(invoice.ExhibitionDate.Date == filterExhibitionDate.Date || filterExhibitionDate == default))
                    continue;

                //remove the time from the date stamp
                invoice.ExhibitionDate = new DateTime(invoice.ExhibitionDate.Year, invoice.ExhibitionDate.Month, invoice.ExhibitionDate.Day);
                sortInvoices.Add(invoice);
            }

            EnvironmentsVariable.filteredInvoices = sortInvoices;
        }
    }
}
