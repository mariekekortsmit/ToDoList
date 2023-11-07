namespace ToDoList.Models.Dtos
{
    public class UpdateItemDto
    {
        public string? Task { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
