using MediatR;
using ToDoList.Models.Dtos;

namespace ToDoList.Features.Todos.Queries
{
    public class GetAllToDoItemsQuery : IRequest<List<ToDoItemDto>>
    {
    }
}
