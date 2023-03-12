namespace InvoicesManager.Models
{
    public class BackUpInfoModel
    {
        public DateTime DateOfCreation { get; set; }
        public string BackUpVersion { get; set; }
        public string BackUpSize { get; set; }

        public int EntityCountInvoices { get; set; }
        public int EntityCountNotes { get; set; }

        public string BackUpName { get; set; }
        public string BackUpPath { get; set; }
    }
}
