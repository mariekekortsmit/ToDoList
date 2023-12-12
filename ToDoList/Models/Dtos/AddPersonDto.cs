using System.Text.Json.Serialization;
using ToDoList.Models.Entities;

namespace ToDoList.Models.Dtos
{
    public class AddPersonDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
