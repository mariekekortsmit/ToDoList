using MediatR;
using ToDoList.Models.Dtos;

namespace ToDoList.ToDos.Queries.Requests
{
    public class GetToDoItems : IRequest<List<ToDoItemDto>>
    {
    }
}
