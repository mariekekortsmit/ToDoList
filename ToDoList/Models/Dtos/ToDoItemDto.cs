namespace ToDoList.Models.Dtos
{
    public class ToDoItemDto
    {
        public Guid Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
