using InvoicesManager.Classes;
using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InvoicesManager.Core
{
    public class NotebookSystem
    {
        public static void Init()
        {
            EnvironmentsVariable.Notebook.Notebooks.Clear();

            string json = File.ReadAllText(EnvironmentsVariable.PathNotebooks + EnvironmentsVariable.NotebooksJsonFileName);

            if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                EnvironmentsVariable.Notebook = JsonConvert.DeserializeObject<NotebookModel>(json);
        }

        public static void AddNotebook(NoteModel newNote)
        {
            EnvironmentsVariable.Notebook.Notebooks.Add(newNote);

            SaveIntoJsonFile();
        }

        public static void EditNotebook(NoteModel oldNote, NoteModel newNote)
        {
            EnvironmentsVariable.Notebook.Notebooks.Remove(oldNote);
            EnvironmentsVariable.Notebook.Notebooks.Add(newNote);

            SaveIntoJsonFile();
        }

        public static void RemoveNotebook(NoteModel oldNote)
        {
            EnvironmentsVariable.Notebook.Notebooks.Remove(oldNote);

            SaveIntoJsonFile();
        }


        private static void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathNotebooks + EnvironmentsVariable.NotebooksJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.Notebook, Formatting.Indented));
        }
    }
}
