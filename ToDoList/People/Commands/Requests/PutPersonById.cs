using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;

namespace ToDoList.People.Commands.Requests
{
    public class PutPersonById : IRequest<bool>
    {
        [FromBody]
        public UpdatePersonDto Person { get; }
        [FromRoute]
        public Guid Id { get; }

        public PutPersonById(Guid id, UpdatePersonDto person)
        {
            Person = person;
            Id = id;
        }
    }
}
