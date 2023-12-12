using System.Text.Json.Serialization;
using ToDoList.Models.Dtos;

namespace ToDoList.Models.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Navigation property for the many-to-many relationship
        public List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();

        public PersonDto ToDto()
        {
            return new PersonDto
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }
    // TODO: remove the item from the person if the item is removed
    // TODO; remove the person from the item if the person is removed
    // TODO; make the todoitems visible in the person body
    // TODO: check if you get people/get items when you've associted them, it says People null or Items null. Change to not show at all.
}
