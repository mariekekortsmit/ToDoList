using MediatR;
using ToDoList.Models.Dtos;

namespace ToDoList.People.Queries.Requests
{
    public class GetPeople : IRequest<List<PersonDto>>
    {
    }
}
