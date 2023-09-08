namespace InvoicesManager.Models
{
    public class NotebookModel
    {
        public List<NoteModel> Notebook { get; set; } = new List<NoteModel>();
    }

    public class NoteModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastEditDate { get; set; }
    }
}
