namespace InvoicesManager.Models
{
    public class InvoiceModel
    {
        //Rules
        public bool ShouldSerializeOpenInvoiceText() { return false; }
        public bool ShouldSerializeStringExhibitionDate() { return false; }
        public bool ShouldSerializeStringCaptureDate() { return false; }
        public bool ShouldSerializeStringMoneyTotal() { return false; }
        public bool ShouldSerializeStringTags() { return false; }
        public bool ShouldSerializeStringPaidState() { return false; }
        public bool ShouldSerializeStringMoneyState() { return false; }
        public bool ShouldSerializeStringImportanceState() { return false; }

        //Model
        public string FileID { get; set; } 
        public DateTime CaptureDate { get; set; } 
        public DateTime ExhibitionDate { get; set; }     
        public string Reference { get; set; } 
        public string DocumentType { get; set; }
        public string Organization { get; set; }
        public string InvoiceNumber { get; set; }
        public string[] Tags { get; set; }
        public ImportanceStateEnum ImportanceState { get; set; } // { VeryImportant, Important, Neutral, Unimportant }
        public MoneyStateEnum MoneyState { get; set; } // { Paid , Received,  NoInvoice }
        public PaidStateEnum PaidState { get; set; } // { Paid , Unpaid,  NoInvoice }
        public double MoneyTotal { get; set; }


        //Type Converter
        public string OpenInvoiceText { get; } = Application.Current.Resources["open"] as string;
        public string StringExhibitionDate { get { return ExhibitionDate.ToString("yyyy.MM.dd"); } }
        public string StringCaptureDate { get { return CaptureDate.ToString("yyyy.MM.dd"); } }
        public string StringMoneyTotal { get { return String.Format("{0:0.0,0}", MoneyTotal) + EnvironmentsVariable.MoneyUnit.ToString(); } }
        public string StringTags { get { return string.Join(", ", Tags); } }
        public string StringPaidState 
        { 
            get 
            {
                string value = PaidState switch
                {
                    PaidStateEnum.Paid => Application.Current.Resources["paid"] as string,
                    PaidStateEnum.Unpaid => Application.Current.Resources["unpaid"] as string,
                    PaidStateEnum.NoInvoice => Application.Current.Resources["noInvoice"] as string,
                    _ => throw new Exception("Invalid PaidStateEnum"),
                };
                
                return value;
            } 
        }

        public string StringMoneyState
        {
            get
            {
                string value = MoneyState switch
                {
                    MoneyStateEnum.Paid => Application.Current.Resources["paid"] as string,
                    MoneyStateEnum.Received => Application.Current.Resources["received"] as string,
                    MoneyStateEnum.NoInvoice => Application.Current.Resources["noInvoice"] as string,
                    _ => throw new Exception("Invalid MoneyStateEnum"),
                };

                return value;
            }
        }

        public string StringImportanceState
        {
            get
            {
                string value = ImportanceState switch
                {
                    ImportanceStateEnum.VeryImportant => Application.Current.Resources["veryImportant"] as string,
                    ImportanceStateEnum.Important => Application.Current.Resources["important"] as string,
                    ImportanceStateEnum.Neutral => Application.Current.Resources["neutral"] as string,
                    ImportanceStateEnum.Unimportant => Application.Current.Resources["unimportant"] as string,
                    _ => throw new Exception("Invalid ImportanceStateEnum"),
                };
                
                return value;
            }
        }

    }
}