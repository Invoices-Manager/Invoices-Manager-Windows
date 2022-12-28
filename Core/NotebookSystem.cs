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

            string json = File.ReadAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName);

            if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                EnvironmentsVariable.Notebook = JsonConvert.DeserializeObject<NotebookModel>(json);
        }

        public static void AddNote(NoteModel newNote)
        {
            EnvironmentsVariable.Notebook.Notebooks.Add(newNote);

            SaveIntoJsonFile();
        }

        public static void EditNote(NoteModel editNote)
        {
            NoteModel note = EnvironmentsVariable.Notebook.Notebooks.Find(x => x.Id == editNote.Id);
            note.Name = editNote.Name;
            note.Value = editNote.Value;
            note.LastEditDate = DateTime.Now;

            SaveIntoJsonFile();
        }

        public static void RemoveNote(NoteModel oldNote)
        {
            EnvironmentsVariable.Notebook.Notebooks.Remove(oldNote);

            SaveIntoJsonFile();
        }


        private static void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.Notebook, Formatting.Indented));
        }
    }
}
