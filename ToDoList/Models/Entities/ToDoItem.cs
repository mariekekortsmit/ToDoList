using ToDoList.Models.Dtos;

namespace ToDoList.Models.Entities
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public ToDoItemDto ToDto()
        {
            return new ToDoItemDto
            {
                Id = Id,
                Task = Task,
                IsCompleted = IsCompleted
            };
        }
    }
}
