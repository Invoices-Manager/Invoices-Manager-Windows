namespace InvoicesManager.Windows
{
    public partial class NotebookWindow : Window
    {
        public NotebookWindow()
            => InitializeComponent();

        private NoteModel selectedNote;
        private Button correspondingButton;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // GenerateDebugNotebooks();
            LoadNotebooks();
            Bttn_SaveNote.IsEnabled = false;
            Bttn_DeleteNote.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
            => EnvironmentsVariable.Window_Notebook_IsClosed = true;

        private void GenerateDebugNotebooks()
        {
            NotebookModel notebook = new NotebookModel();
            
            for (int i = 0; i < 125; i++)
            {
                NoteModel noteModel = new NoteModel()
                {
                    Name = $"Notebook {i}",
                    Value = $"Notebook {i} value",
                    CreationDate = DateTime.Now - TimeSpan.FromDays(i),
                    LastEditDate = DateTime.Now
                };

                notebook.Notebook.Add(noteModel);
            }

            EnvironmentsVariable.Notebook = notebook;
        }

        private void LoadNotebooks()
        {
            try
            {
                sP_Notes.Items.Clear();

                NotebookSystem nSys = new NotebookSystem();
                nSys.Init();

                foreach (NoteModel note in EnvironmentsVariable.Notebook.Notebook)
                    sP_Notes.Items.Add(note);

                ClearTbs();

                Bttn_SaveNote.IsEnabled = false;
                Bttn_DeleteNote.IsEnabled = false;
                correspondingButton = null;
                selectedNote = null;
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_View, "Error while loading the notebooks, err: " + ex.Message);
            }
        }
        
        private void Bttn_LoadNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = ((NoteModel)((FrameworkElement)sender).DataContext).Id;
                selectedNote = EnvironmentsVariable.Notebook.Notebook.Find(note => note.Id == id);
                if (sender is Button)
                    correspondingButton = (Button)sender;

                Tb_Note_Title.Text = selectedNote.Name;
                Tb_Note_Value.Text = selectedNote.Value;

                if (correspondingButton != null)
                {
                    Bttn_SaveNote.IsEnabled = true;
                    Bttn_DeleteNote.IsEnabled = true;
                    Tb_Note_Title.IsReadOnly = false;
                    Tb_Note_Value.IsReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_View, "Error while loading the note, err: " + ex.Message);
            }
        }

        private void Bttn_DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (correspondingButton is null)
                    return;

                correspondingButton = null;
                NotebookSystem nSys = new NotebookSystem();
                nSys.RemoveNote(selectedNote);
                Tb_Note_Title.Text = "";
                Tb_Note_Value.Text = "";
                LoadNotebooks();
                CheckButtonsState();
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_View, "Error while deleting the note, err: " + ex.Message);
            }
        }

        private void CheckButtonsState()
        {
            if (EnvironmentsVariable.Notebook.Notebook.Count == 0 || correspondingButton is null)
            {
                Bttn_SaveNote.IsEnabled = false;
                Bttn_DeleteNote.IsEnabled = false;
                Tb_Note_Title.IsReadOnly = true;
                Tb_Note_Value.IsReadOnly = true;
            }
            else
            {
                Bttn_SaveNote.IsEnabled = true;
                Bttn_DeleteNote.IsEnabled = true;
                Tb_Note_Title.IsReadOnly = false;
                Tb_Note_Value.IsReadOnly = false;
            }

        }

        private void ClearTbs()
        {
            Tb_Note_Title.Text = "";
            Tb_Note_Value.Text = "";
        }

        private void Bttn_SaveNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (correspondingButton is null)
                    return;

                correspondingButton.Content = Tb_Note_Title.Text;
                selectedNote.Name = Tb_Note_Title.Text;
                selectedNote.Value = Tb_Note_Value.Text;
                selectedNote.LastEditDate = DateTime.Now;

                NotebookSystem nSys = new NotebookSystem();
                nSys.EditNote(selectedNote);
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Notebook_View, "Error while saving the note, err: " + ex.Message);
            }
        }

        private void Bttn_CreateNote_Click(object sender, RoutedEventArgs e)
        {
            NoteModel note = new NoteModel()
            {
                Name = $"Note {EnvironmentsVariable.Notebook.Notebook.Count}",
                Value = $"",
                CreationDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            NotebookSystem nSys = new NotebookSystem();
            nSys.AddNote(note);
            LoadNotebooks();
            CheckButtonsState();
        }

        private void Bttn_RefreshNotebook_Click(object sender, RoutedEventArgs e)
        {
            LoadNotebooks();
            CheckButtonsState();
        }
    }
}
