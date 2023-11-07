namespace ToDoList.Models.Dtos
{
    public class AddItemDto
    {
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
