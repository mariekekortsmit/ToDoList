using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;

namespace ToDoList.ToDos.Queries.Requests
{
    public class GetToDoItemById : IRequest<ToDoItemDto?>
    {
        [FromRoute]
        public Guid Id { get; }

        public GetToDoItemById(Guid id)
        {
            Id = id;
        }
    }
}