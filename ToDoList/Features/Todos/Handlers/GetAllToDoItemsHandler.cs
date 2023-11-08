using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Features.Todos.Queries;

namespace ToDoList.Features.Todos.Handlers
{
    public class GetAllToDoItemsHandler : IRequestHandler<GetAllToDoItemsQuery, List<ToDoItemDto>>
    {
        private readonly IToDoDatabase _database;

        public GetAllToDoItemsHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public Task<List<ToDoItemDto>> Handle(GetAllToDoItemsQuery request, CancellationToken cancellationToken)
        {
            var items = _database.GetAll();
            return Task.FromResult(items);
        }
    }
}
