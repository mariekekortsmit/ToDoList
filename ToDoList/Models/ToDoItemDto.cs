namespace ToDoList.Models
{
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
