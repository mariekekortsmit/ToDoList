namespace ToDoList
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public ToDoItemDto ToDto()
        {
            return new ToDoItemDto { 
                Id = Id, 
                Task = Task, 
                IsCompleted = IsCompleted 
            };
        }
    }

    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class AddItemDto
    {
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class UpdateItemDto
    {
        public string? Task { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
