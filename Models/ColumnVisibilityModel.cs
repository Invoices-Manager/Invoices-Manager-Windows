namespace InvoicesManager.Models
{
    public class ColumnVisibilityModel
    {
        public bool IsVisibleColumnOpen { get; set; } = true;
        public bool IsVisibleColumnDateOfExhibition { get; set; } = true;
        public bool IsVisibleColumnOrganization { get; set; } = true;
        public bool IsVisibleColumnDocumentType { get; set; } = true;
        public bool IsVisibleColumnInvoiceNo { get; set; } = true;
        public bool IsVisibleColumnReference { get; set; } = true;
        public bool IsVisibleColumnMoneyTotal { get; set; } = true;
        public bool IsVisibleColumnImportanceState { get; set; } = true;
        public bool IsVisibleColumnMoneyState { get; set; } = true;
        public bool IsVisibleColumnPaidState { get; set; } = true;
        public bool IsVisibleColumnTags { get; set; } = true;
        public bool IsVisibleColumnDateOfCapture { get; set; } = true;
    }
}
