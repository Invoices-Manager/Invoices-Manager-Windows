namespace InvoicesManager.Classes.Enums
{
    public enum MoneyStateEnum
    {
        Paid,
        Received,
        NoInvoice,
        FilterPlaceholder
    }

    public class MoneyState
    {
        public static string EnumAsString(MoneyStateEnum moneyState)
        {
            return moneyState switch
            {
                MoneyStateEnum.Paid => Application.Current.Resources["paid"] as string,
                MoneyStateEnum.Received => Application.Current.Resources["received"] as string,
                MoneyStateEnum.NoInvoice => Application.Current.Resources["noInvoice"] as string,
                MoneyStateEnum.FilterPlaceholder => String.Empty,
                _ => throw new Exception("Invalid MoneyStateEnum"),
            };
        }

        public static MoneyStateEnum StringAsEnum(string @enum)
        {
            if (@enum == Application.Current.Resources["paid"] as string)
                return MoneyStateEnum.Paid;
            
            if (@enum == Application.Current.Resources["received"] as string)
                return MoneyStateEnum.Received;
            
            if (@enum == Application.Current.Resources["noInvoice"] as string)
                return MoneyStateEnum.NoInvoice;
            
            if (@enum == String.Empty)
                return MoneyStateEnum.FilterPlaceholder;
            
            throw new Exception("Invalid MoneyStateEnum");
        }
    }
}