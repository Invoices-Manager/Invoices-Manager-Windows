using Newtonsoft.Json.Linq;

namespace InvoicesManager.Core
{
    public class NotebookSystem
    {
        readonly EncryptionSystem _es = new (EnvironmentsVariable.GetUserPassword(), EnvironmentsVariable.GetUserSalt());
        
        public void Init()
        {
            try
            {
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Notebook_System, "Notebook init has been started.");

                EnvironmentsVariable.Notebook.Notebook.Clear();

                string json = NoteWebSystem.GetAll();

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    foreach (NoteModel note in JsonConvert.DeserializeObject<NotebookModel>(json).Notebook)
                    {
                        //decrypt notebook data
                        note.Name = _es.DecryptString(note.Name);
                        note.Value = _es.DecryptString(note.Value);

                        EnvironmentsVariable.Notebook.Notebook.Add(note);
                    }

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
                //encrypt notebook data
                NoteModel encryptedNote = new NoteModel()
                {
                    Name = _es.EncryptString(newNote.Name),
                    Value = _es.EncryptString(newNote.Value)
                };

                //save into web
                int id = NoteWebSystem.Add(encryptedNote);

                //if id == -1 => then error
                if (id == -1)
                    throw new Exception("Error adding note via api. ID = -1");

                newNote.Id = id;
                //save into env
                EnvironmentsVariable.Notebook.Notebook.Add(newNote);
               
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
                //save into env
                NoteModel note = EnvironmentsVariable.Notebook.Notebook.Find(x => x.Id == editNote.Id);
                note.Name = editNote.Name;
                note.Value = editNote.Value;
                note.LastEditDate = DateTime.Now;

                //encrypt notebook data
                NoteModel encryptedNote = new NoteModel()
                {
                    Id = note.Id,
                    Name = _es.EncryptString(note.Name),
                    Value = _es.EncryptString(note.Value),
                    CreationDate = note.CreationDate,
                    LastEditDate = note.LastEditDate
                };

                //save into web
                if (!NoteWebSystem.Edit(encryptedNote))
                    throw new Exception("Error editing note via api.");

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
                //save into web
                if (!NoteWebSystem.Delete(oldNote.Id))
                    throw new Exception("Error removing note via api.");
  
                //save into env
                EnvironmentsVariable.Notebook.Notebook.Remove(oldNote);

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
    }
}
