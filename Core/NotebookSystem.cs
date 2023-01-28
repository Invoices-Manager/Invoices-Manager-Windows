namespace InvoicesManager.Core
{
    public class NotebookSystem
    {
        public void Init()
        {
            try
            {
                EnvironmentsVariable.Notebook.Notebook.Clear();

                string json = File.ReadAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName);

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    EnvironmentsVariable.Notebook = JsonConvert.DeserializeObject<NotebookModel>(json);
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Notebook_System, ex.Message);
            }
        }

        public void AddNote(NoteModel newNote)
        {
            try
            {
                EnvironmentsVariable.Notebook.Notebook.Add(newNote);

                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Notebook_System, $"A new note has been added. [{newNote.Id}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Notebook_System, ex.Message);
            }
        }

        public void EditNote(NoteModel editNote)
        {
            try
            {
                NoteModel note = EnvironmentsVariable.Notebook.Notebook.Find(x => x.Id == editNote.Id);
                note.Name = editNote.Name;
                note.Value = editNote.Value;
                note.LastEditDate = DateTime.Now;

                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Notebook_System, $"A note has been edited. [{editNote.Id}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Notebook_System, ex.Message);
            }
        }

        public void RemoveNote(NoteModel oldNote)
        {
            try
            {
                EnvironmentsVariable.Notebook.Notebook.Remove(oldNote);

                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Notebook_System, $"A note has been removed. [{oldNote.Id}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Notebook_System, ex.Message);
            }
        }

        public bool CheckIfNoteExist(NoteModel note)
        {
            return EnvironmentsVariable.Notebook.Notebook.Exists(x => x.Id == note.Id);
        }

        public bool CheckIfNoteHasChanged(NoteModel note)
        {
            NoteModel noteFromList = EnvironmentsVariable.Notebook.Notebook.Find(x => x.Id == note.Id);

            return noteFromList.Name != note.Name || noteFromList.Value != note.Value;
        }
        
        private void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.Notebook, Formatting.Indented));
        }
    }
}
