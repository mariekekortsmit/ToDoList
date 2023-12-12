using ToDoList.Models.Dtos;

namespace ToDoList.Models.Entities
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        // Navigation property for the many-to-many relationship
        public List<Person> People { get; set; } = new List<Person>();

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
