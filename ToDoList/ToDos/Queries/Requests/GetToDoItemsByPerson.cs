using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;

namespace ToDoList.ToDos.Queries.Requests
{
    public class GetToDoItemsByPerson : IRequest<List<ToDoItemDto>>
    {
        [FromRoute]
        public Guid PersonId { get; }

        public GetToDoItemsByPerson(Guid personId)
        {
            PersonId = personId;
        }
    }
}
