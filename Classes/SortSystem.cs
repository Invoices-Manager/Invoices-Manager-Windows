using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoicesManager.Classes
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
            this.filterReference = filterReference;
            this.filterInvoiceNumber = filterInvoiceNumber;
            this.filterOrganization = filterOrganization;
            this.filterDocumentType = filterDocumentType;
            this.filterExhibitionDate = filterExhibitionDate;
        }

        public List<InvoiceModel> Sort()
        {
            foreach (var invoice in allInvoices)
            {
                if (!(invoice.Reference.Contains(filterReference) || filterReference == String.Empty))
                    continue;
                if (!(invoice.InvoiceNumber.Contains(filterInvoiceNumber) || filterInvoiceNumber == String.Empty))
                    continue;
                if (!(invoice.Organization == filterOrganization || filterOrganization == "-1"))
                    continue;
                if (!(invoice.DocumentType == filterDocumentType || filterDocumentType == "-1"))
                    continue;
                if (!(invoice.ExhibitionDate.Date == filterExhibitionDate.Date || filterExhibitionDate == default))
                    continue;

                //remove the time from the date stamp
                invoice.ExhibitionDate = new DateTime(invoice.ExhibitionDate.Year, invoice.ExhibitionDate.Month, invoice.ExhibitionDate.Day);
                sortInvoices.Add(invoice);
            }

            return sortInvoices;
        }
    }
}
