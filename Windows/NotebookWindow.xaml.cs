using InvoicesManager.Classes;
using InvoicesManager.Core;
using InvoicesManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InvoicesManager.Windows
{
    public partial class NotebookWindow : Window
    {
        public NotebookWindow()
        {
            InitializeComponent();
        }

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
                    NotebookName = $"Notebook {i}",
                    NotebookValue = $"Notebook {i} value",
                    NotebookCreationDate = DateTime.Now - TimeSpan.FromDays(i),
                    NotebookLastEditDate = DateTime.Now
                };

                notebook.Notebooks.Add(noteModel);
            }

            EnvironmentsVariable.Notebook = notebook;
        }

        private void LoadNotebooks()
        {
            sP_NoteBooks.Items.Clear();
            foreach (NoteModel note in EnvironmentsVariable.Notebook.Notebooks)
                sP_NoteBooks.Items.Add(note);
        }
        
        private void Bttn_LoadNote_Click(object sender, RoutedEventArgs e)
        {
            Tb_Note_Title.Text = ((NoteModel)((FrameworkElement)sender).DataContext).NotebookName;
            Tb_Note_Value.Text = ((NoteModel)((FrameworkElement)sender).DataContext).NotebookValue;
        }

        private void Bttn_DeleteNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_SaveNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bttn_CreateNote_Click(object sender, RoutedEventArgs e)
        {
            NoteModel note = new NoteModel()
            {
                NotebookName = $"Notebook {EnvironmentsVariable.Notebook.Notebooks.Count}",
                NotebookValue = $"",
                NotebookCreationDate = DateTime.Now,
                NotebookLastEditDate = DateTime.Now
            };

            NotebookSystem.AddNote(note);
            LoadNotebooks();
        }
    }
}
