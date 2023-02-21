namespace InvoicesManager.Classes.Converter
{
    public class NegativeOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string i && i == ("-1,00" + EnvironmentsVariable.MoneyUnit))
                return "";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}