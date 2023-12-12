using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.People.Commands.Requests
{
    public class AddPerson : IRequest<Person>
    {
        [FromBody]
        public AddPersonDto Person { get; }

        public AddPerson(AddPersonDto person)
        {
            Person = person;
        }
    }
}
