using System.Text.Json.Serialization;
using ToDoList.Models.Entities;

namespace ToDoList.Models.Dtos
{
    public class UpdatePersonDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
