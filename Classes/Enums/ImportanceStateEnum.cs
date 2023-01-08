using System;
using System.Windows;

namespace InvoicesManager.Classes.Enums
{
    public enum ImportanceStateEnum
    {
        VeryImportant,
        Important,
        Neutral,
        Unimportant,
        FilterPlaceholder
    }

    public class ImportanceState
    {
        public static string EnumAsString(ImportanceStateEnum importanceState)
        {
            return importanceState switch
            {
                ImportanceStateEnum.VeryImportant => Application.Current.Resources["veryImportant"] as string,
                ImportanceStateEnum.Important => Application.Current.Resources["important"] as string,
                ImportanceStateEnum.Neutral => Application.Current.Resources["neutral"] as string,
                ImportanceStateEnum.Unimportant => Application.Current.Resources["unimportant"] as string,
                ImportanceStateEnum.FilterPlaceholder => String.Empty,
                _ => throw new Exception("Invalid ImportanceStateEnum"),
            };
        }

        public static ImportanceStateEnum StringAsEnum(string @enum)
        {
            if (@enum == Application.Current.Resources["veryImportant"] as string)
                return ImportanceStateEnum.VeryImportant;

            if (@enum == Application.Current.Resources["important"] as string)
                return ImportanceStateEnum.Important;

            if (@enum == Application.Current.Resources["neutral"] as string)
                return ImportanceStateEnum.Neutral;

            if (@enum == Application.Current.Resources["unimportant"] as string)
                return ImportanceStateEnum.Unimportant;

            if (@enum == String.Empty)
                return ImportanceStateEnum.FilterPlaceholder;

            throw new Exception("Invalid ImportanceStateEnum");
        }
    }
}
