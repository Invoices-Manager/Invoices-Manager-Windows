namespace InvoicesManager.Core
{
    public class NotebookSystem
    {
        public void Init()
        {
            try
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Notebook_System, "Notebook init has been started.");

                EnvironmentsVariable.Notebook.Notebook.Clear();

                string json = File.ReadAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName);

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    EnvironmentsVariable.Notebook = JsonConvert.DeserializeObject<NotebookModel>(json);
                
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Notebook_System, "Notebook system has been initialized.");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_System, "Error initializing Notebook System, err: " + ex.Message);
            }
        }

        public void AddNote(NoteModel newNote)
        {
            try
            {
                EnvironmentsVariable.Notebook.Notebook.Add(newNote);

                SaveIntoJsonFile();
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Notebook_System, $"A new note has been added. [{newNote.Id}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_System, "Error adding note, err: " + ex.Message);
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
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Notebook_System, $"A note has been edited. [{editNote.Id}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_System, "Error editing note, err: " + ex.Message);
            }
        }

        public void RemoveNote(NoteModel oldNote)
        {
            try
            {
                EnvironmentsVariable.Notebook.Notebook.Remove(oldNote);

                SaveIntoJsonFile();
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Notebook_System, $"A note has been removed. [{oldNote.Id}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_System, "Error removing note, err: " + ex.Message);
            }
        }

        public bool CheckIfNoteExist(NoteModel note)
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Notebook_System, "CheckIfNoteExist() has been called");
#endif
            return EnvironmentsVariable.Notebook.Notebook.Exists(x => x.Id == note.Id);
        }

        public bool CheckIfNoteHasChanged(NoteModel note)
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Notebook_System, "CheckIfNoteHasChanged() has been called");
#endif
            NoteModel noteFromList = EnvironmentsVariable.Notebook.Notebook.Find(x => x.Id == note.Id);
            return noteFromList.Name != note.Name || noteFromList.Value != note.Value;
        }
        
        private void SaveIntoJsonFile()
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Notebook_System, "SaveIntoJsonFile() has been called");
#endif
            try
            {
                File.WriteAllText(EnvironmentsVariable.PathNotebook + EnvironmentsVariable.NotebooksJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.Notebook, Formatting.Indented));
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_System, $"Error saving changes to the notebook file, err: {ex.Message}");
            }
        }
    }
}
