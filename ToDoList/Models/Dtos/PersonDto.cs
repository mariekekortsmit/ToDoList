using System.Text.Json.Serialization;
using ToDoList.Models.Entities;

namespace ToDoList.Models.Dtos
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
