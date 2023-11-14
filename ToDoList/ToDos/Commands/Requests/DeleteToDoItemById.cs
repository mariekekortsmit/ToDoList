using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.ToDos.Commands.Requests
{
    public class DeleteToDoItemById : IRequest<bool>
    {
        [FromRoute]
        public Guid Id { get; }

        public DeleteToDoItemById(Guid id)
        {
            Id = id;
        }
    }
}
