using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.People.Commands.Requests
{
    public class AddPersonToToDoItem : IRequest<bool>
    {
        [FromRoute]
        public Guid PersonId { get; }
        [FromRoute]
        public Guid ItemId { get; }

        public AddPersonToToDoItem(Guid itemId, Guid personId)
        {
            PersonId = personId;
            ItemId = itemId;
        }
    }
}
