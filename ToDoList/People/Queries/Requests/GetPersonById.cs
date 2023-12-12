using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;

namespace ToDoList.People.Queries.Requests
{
    public class GetPersonById : IRequest<PersonDto?>
    {
        [FromRoute]
        public Guid Id { get; }

        public GetPersonById(Guid id)
        {
            Id = id;
        }
    }
}