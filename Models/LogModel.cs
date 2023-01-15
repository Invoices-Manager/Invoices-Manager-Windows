using System;

namespace InvoicesManager.Models
{
    public class LogModel
    {
        public string FullLog { get; set; }
        public SubLogModel LogData { get; set; }
    }

    public class SubLogModel
    {
        public DateTime DateOfTheEvent { get; set; }
        public string State { get; set; }
        public string Prefix { get; set; }
        public string Log { get; set; }
    }
}
