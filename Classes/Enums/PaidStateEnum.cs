namespace InvoicesManager.Classes.Enums
{
    public enum PaidStateEnum
    {
        Paid,
        Unpaid,
        NoInvoice,
        FilterPlaceholder
    }

    public class PaidState
    {
        public static string EnumAsString(PaidStateEnum paidState)
        {
            return paidState switch
            {
                PaidStateEnum.Paid => Application.Current.Resources["paid"] as string,
                PaidStateEnum.Unpaid => Application.Current.Resources["unpaid"] as string,
                PaidStateEnum.NoInvoice => Application.Current.Resources["noInvoice"] as string,
                PaidStateEnum.FilterPlaceholder => String.Empty,
                _ => throw new Exception("Invalid PaidStateEnum"),
            };
        }

        public static PaidStateEnum StringAsEnum(string @enum)
        {
            if (@enum == Application.Current.Resources["paid"] as string)
                return PaidStateEnum.Paid;

            if (@enum == Application.Current.Resources["unpaid"] as string)
                return PaidStateEnum.Unpaid;

            if (@enum == Application.Current.Resources["noInvoice"] as string)
                return PaidStateEnum.NoInvoice;

            if (@enum == String.Empty)
                return PaidStateEnum.FilterPlaceholder;

            throw new Exception("Invalid PaidStateEnum");
        }
    }
}