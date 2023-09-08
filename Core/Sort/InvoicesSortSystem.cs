namespace InvoicesManager.Core.Sort
{
    public class InvoicesSortSystem
    {
        private readonly List<InvoiceModel> allInvoices;
        private readonly string filterReference = string.Empty;
        private readonly string filterInvoiceNumber = string.Empty;
        private readonly string filterOrganization = "-1";
        private readonly string filterDocumentType = "-1";
        private readonly DateTime filterExhibitionDate = default;
        private readonly PaidStateEnum filterPaidState = PaidStateEnum.FilterPlaceholder;
        private readonly MoneyStateEnum filterMoneyState = MoneyStateEnum.FilterPlaceholder;
        private readonly ImportanceStateEnum filterImportanceState = ImportanceStateEnum.FilterPlaceholder;
        private readonly double filterMoneyTotal = double.MinValue; // -1 is not possible because it is a valid value
        private readonly string filterTags = string.Empty;

        public InvoicesSortSystem(List<InvoiceModel> allInvoices,
                                        string filterReference,
                                        string filterInvoiceNumber,
                                        string filterOrganization,
                                        string filterDocumentType,
                                        DateTime filterExhibitionDate,
                                        PaidStateEnum filterPaidState,
                                        MoneyStateEnum filterMoneyState,
                                        ImportanceStateEnum filterImportanceState,
                                        double filterMoneyTotal,
                                        string filterTags)
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
            this.filterTags = filterTags;
        }

        public void Sort()
        {
            try
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.InvoicesSort_System, $"Start sorting invoices");
                DateTime startTime = DateTime.Now;

                EnvironmentsVariable.FilteredInvoices = allInvoices
                              .Where(x => x.Reference.ToLower().Contains(filterReference) || string.IsNullOrEmpty(filterReference))
                              .Where(x => x.InvoiceNumber.ToLower().Contains(filterInvoiceNumber) || string.IsNullOrEmpty(filterInvoiceNumber))
                              .Where(x => x.Organization.ToLower().Equals(filterOrganization) || filterOrganization is "-1")
                              .Where(x => x.DocumentType.ToLower().Equals(filterDocumentType) || filterDocumentType is "-1")
                              .Where(x => x.ExhibitionDate.Date == filterExhibitionDate.Date || filterExhibitionDate == default)
                              .Where(x => x.PaidState == filterPaidState || filterPaidState == PaidStateEnum.FilterPlaceholder)
                              .Where(x => x.MoneyState == filterMoneyState || filterMoneyState == MoneyStateEnum.FilterPlaceholder)
                              .Where(x => x.ImportanceState == filterImportanceState || filterImportanceState == ImportanceStateEnum.FilterPlaceholder)
                              .Where(x => x.MoneyTotalDouble == filterMoneyTotal || filterMoneyTotal == double.MinValue)
                              .Where(x => x.Tags.Select(y => y.ToLower()).Contains(filterTags.ToLower()) || string.IsNullOrEmpty(filterTags))
                              .ToList();

                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.InvoicesSort_System, $"Stop sorting invoices, took {(DateTime.Now - startTime).TotalMilliseconds} ms");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.InvoicesSort_System, $"Error while sorting invoices, err: {ex.Message}");
            }
        }
    }
}
