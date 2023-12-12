using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.People.Commands.Requests
{
    public class DeletePersonById : IRequest<bool>
    {
        [FromRoute]
        public Guid Id { get; }

        public DeletePersonById(Guid id)
        {
            Id = id;
        }
    }
}
