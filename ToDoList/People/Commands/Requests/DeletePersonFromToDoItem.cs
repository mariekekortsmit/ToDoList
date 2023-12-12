using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.People.Commands.Requests
{
    public class DeletePersonFromToDoItem : IRequest<bool>
    {
        [FromRoute]
        public Guid PersonId { get; }
        public Guid ItemId { get; }

        public DeletePersonFromToDoItem(Guid itemId, Guid personId)
        {
            PersonId = personId;
            ItemId = itemId;
        }
    }
}
