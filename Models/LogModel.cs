namespace InvoicesManager.Models
{
    public class LogModel
    {
        public string FullLog { get; set; }
        public SubLogModel LogData { get; set; }
        
        //static text
        public string CopyText { get; } = Application.Current.Resources["copy"] as string;

        //Rule 
        public bool ShouldSerializeCopyText() { return false; }
    }

    public class SubLogModel
    {
        public DateTime DateOfTheEvent { get; set; }
        public string State { get; set; }
        public string Prefix { get; set; }
        public string Log { get; set; }
    }
}
