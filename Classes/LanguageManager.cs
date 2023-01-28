namespace InvoicesManager.Classes
{
    public class LanguageManager
    {
        public static void Init()
        {
            ResourceDictionary dict = new ResourceDictionary();

            switch (EnvironmentsVariable.UILanguage)
            {
                case "English":
                    dict.Source = new Uri("..\\Resources\\Languages\\Language_en-US.xaml", UriKind.Relative);
                    break;

                case "German":
                    dict.Source = new Uri("..\\Resources\\Languages\\Language_de-DE.xaml", UriKind.Relative);
                    break;

                default:
                    dict.Source = new Uri("..\\Resources\\Languages\\Language_en-US.xaml", UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
