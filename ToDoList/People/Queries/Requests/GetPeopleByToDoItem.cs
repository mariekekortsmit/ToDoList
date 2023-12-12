using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos;

namespace ToDoList.People.Queries.Requests
{
    public class GetPeopleByToDoItem : IRequest<List<PersonDto>>
    {
        [FromRoute]
        public Guid ItemId { get; }

        public GetPeopleByToDoItem(Guid itemId)
        {
            ItemId = itemId;
        }
    }
}
