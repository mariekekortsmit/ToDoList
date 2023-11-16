using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.ToDos.Commands.Requests
{
    public class PutToDoItemById : IRequest<bool>
    {
        [FromBody]
        public UpdateItemDto Item { get; }
        [FromRoute]
        public Guid Id { get; }

        public PutToDoItemById(Guid id, UpdateItemDto item)
        {
            Item = item;
            Id = id;
        }
    }
}
