using InvoicesManager.Classes;
using InvoicesManager.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace InvoicesManager.Core
{
    public class NotebookSystem
    {
        public static void Init()
        {
            EnvironmentsVariable.Notebook.Notebook.Clear();

            string json = File.ReadAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName);

            if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                EnvironmentsVariable.Notebook = JsonConvert.DeserializeObject<NotebookModel>(json);
        }

        public static void AddNote(NoteModel newNote)
        {
            EnvironmentsVariable.Notebook.Notebook.Add(newNote);

            SaveIntoJsonFile();
        }

        public static void EditNote(NoteModel editNote)
        {
            NoteModel note = EnvironmentsVariable.Notebook.Notebook.Find(x => x.Id == editNote.Id);
            note.Name = editNote.Name;
            note.Value = editNote.Value;
            note.LastEditDate = DateTime.Now;

            SaveIntoJsonFile();
        }

        public static void RemoveNote(NoteModel oldNote)
        {
            EnvironmentsVariable.Notebook.Notebook.Remove(oldNote);

            SaveIntoJsonFile();
        }

        public static bool CheckIfNoteExist(NoteModel note)
        {
            return EnvironmentsVariable.Notebook.Notebook.Exists(x => x.Id == note.Id);
        }

        public static bool CheckIfNoteHasChanged(NoteModel note)
        {
            NoteModel noteFromList = EnvironmentsVariable.Notebook.Notebook.Find(x => x.Id == note.Id);

            return noteFromList.Name != note.Name || noteFromList.Value != note.Value;
        }


        private static void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.Notebook, Formatting.Indented));
        }
    }
}
