using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.ToDos.Commands.Requests
{
    public class AddToDoItem : IRequest<ToDoItem>
    {
        [FromBody]
        public AddItemDto Item { get; }

        public AddToDoItem(AddItemDto item)
        {
            Item = item;
        }
    }
}
