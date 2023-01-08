using InvoicesManager.Classes;
using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using InvoicesManager.Classes.Enums;

namespace InvoicesManager.Core
{
    public class SortSystem
    {
        private readonly List<InvoiceModel> allInvoices;
        private readonly string filterReference = String.Empty;
        private readonly string filterInvoiceNumber = String.Empty;
        private readonly string filterOrganization = "-1";
        private readonly string filterDocumentType = "-1";
        private readonly DateTime filterExhibitionDate = default;
        private PaidStateEnum filterPaidState = PaidStateEnum.FilterPlaceholder;
        private MoneyStateEnum filterMoneyState = MoneyStateEnum.FilterPlaceholder;
        private ImportanceStateEnum filterImportanceState = ImportanceStateEnum.FilterPlaceholder;
        private double filterMoneyTotal = double.MinValue; // -1 is not possible because it is a valid value

        public SortSystem(List<InvoiceModel> allInvoices, 
                                        string filterReference, 
                                        string filterInvoiceNumber, 
                                        string filterOrganization, 
                                        string filterDocumentType, 
                                        DateTime filterExhibitionDate,
                                        PaidStateEnum filterPaidState,
                                        MoneyStateEnum filterMoneyState,
                                        ImportanceStateEnum filterImportanceState,
                                        double filterMoneyTotal)
        {
            this.allInvoices = allInvoices;
            this.filterReference = filterReference.ToLower();
            this.filterInvoiceNumber = filterInvoiceNumber.ToLower();
            this.filterOrganization = filterOrganization.ToLower();
            this.filterDocumentType = filterDocumentType.ToLower();
            this.filterExhibitionDate = filterExhibitionDate;
            this.filterPaidState = filterPaidState;
            this.filterMoneyState = filterMoneyState;
            this.filterImportanceState = filterImportanceState;
            this.filterMoneyTotal = filterMoneyTotal;
        }
        
        public void Sort()
        {
            EnvironmentsVariable.FilteredInvoices = allInvoices
                .Where(x => x.Reference.ToLower().Contains(filterReference) || String.IsNullOrEmpty(filterReference))
                .Where(x => x.InvoiceNumber.ToLower().Contains(filterInvoiceNumber) || String.IsNullOrEmpty(filterInvoiceNumber))
                .Where(x => x.Organization.ToLower().Equals(filterOrganization) || filterOrganization is "-1")
                .Where(x => x.DocumentType.ToLower().Equals(filterDocumentType) || filterDocumentType is "-1")
                .Where(x => x.ExhibitionDate.Date == filterExhibitionDate.Date || filterExhibitionDate == default)
                .Where(x => x.PaidState == filterPaidState || filterPaidState == PaidStateEnum.FilterPlaceholder)
                .Where(x => x.MoneyState == filterMoneyState || filterMoneyState == MoneyStateEnum.FilterPlaceholder)
                .Where(x => x.ImportanceState == filterImportanceState || filterImportanceState == ImportanceStateEnum.FilterPlaceholder)
                .Where(x => x.MoneyTotal == filterMoneyTotal || filterMoneyTotal == double.MinValue)
                .ToList();
        }
    }
}
