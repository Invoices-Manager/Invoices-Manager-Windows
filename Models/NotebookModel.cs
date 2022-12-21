using System;
using System.Collections.Generic;

namespace InvoicesManager.Models
{
    public class NotebookModel
    {
        public List<NoteModel> Notebooks { get; set; } = new List<NoteModel>();
    }

    public class NoteModel
    {
        public string NotebookName { get; set; }
        public string NotebookValue { get; set; }
        public DateTime NotebookCreationDate { get; set; }
        public DateTime NotebookLastEditDate { get; set; }
    }
}
