namespace InvoicesManager.Classes
{
    public class LanguageManager
    {
        public static void Init()
        {
            ResourceDictionary dict = new ResourceDictionary();

            dict.Source = EnvironmentsVariable.UILanguage switch
            {
                "English" => new Uri("..\\Resources\\Languages\\Language_en-US.xaml", UriKind.Relative),
                "German" => new Uri("..\\Resources\\Languages\\Language_de-DE.xaml", UriKind.Relative),
                _ => new Uri("..\\Resources\\Languages\\Language_en-US.xaml", UriKind.Relative),
            };
            
            Application.Current.Resources.MergedDictionaries.Add(dict);
            
            LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Language_System, "Language system has been initialized.");
        }
    }
}
