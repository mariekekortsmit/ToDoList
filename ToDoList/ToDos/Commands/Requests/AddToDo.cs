using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.ToDos.Commands.Requests
{
    public class AddToDo : IRequest<ToDoItem>
    {
        [FromBody]
        public AddItemDto Item { get; }

        public AddToDo(AddItemDto item)
        {
            Item = item;
        }
    }
}
