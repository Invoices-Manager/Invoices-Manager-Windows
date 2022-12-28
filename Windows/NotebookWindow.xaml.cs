using InvoicesManager.Classes;
using InvoicesManager.Core;
using InvoicesManager.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InvoicesManager.Windows
{
    public partial class NotebookWindow : Window
    {
        public NotebookWindow()
        {
            InitializeComponent();
        }

        private NoteModel selectedNote;
        private Button correspondingButton;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // GenerateDebugNotebooks();
            LoadNotebooks();
        }

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

                notebook.Notebooks.Add(noteModel);
            }

            EnvironmentsVariable.Notebook = notebook;
        }

        private void LoadNotebooks()
        {
            sP_Notes.Items.Clear();
            foreach (NoteModel note in EnvironmentsVariable.Notebook.Notebooks)
                sP_Notes.Items.Add(note);
        }
        
        private void Bttn_LoadNote_Click(object sender, RoutedEventArgs e)
        {
            Guid id = ((NoteModel)((FrameworkElement)sender).DataContext).Id;
            selectedNote = EnvironmentsVariable.Notebook.Notebooks.Find(note => note.Id == id);
            if (sender is Button)
                correspondingButton = (Button)sender;

            Tb_Note_Title.Text = selectedNote.Name;
            Tb_Note_Value.Text = selectedNote.Value;
        }

        private void Bttn_DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            NotebookSystem.RemoveNote(selectedNote);
            Tb_Note_Title.Text = "";
            Tb_Note_Value.Text = "";
            LoadNotebooks();
        }

        private void Bttn_SaveNote_Click(object sender, RoutedEventArgs e)
        {
            correspondingButton.Content = Tb_Note_Title.Text;
            selectedNote.Name = Tb_Note_Title.Text;
            selectedNote.Value = Tb_Note_Value.Text;
            selectedNote.LastEditDate = DateTime.Now;
            NotebookSystem.EditNote(selectedNote);
        }

        private void Bttn_CreateNote_Click(object sender, RoutedEventArgs e)
        {
            NoteModel note = new NoteModel()
            {
                Id = Guid.NewGuid(),
                Name = $"Notebook {EnvironmentsVariable.Notebook.Notebooks.Count}",
                Value = $"",
                CreationDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            NotebookSystem.AddNote(note);
            LoadNotebooks();
        }
    }
}
