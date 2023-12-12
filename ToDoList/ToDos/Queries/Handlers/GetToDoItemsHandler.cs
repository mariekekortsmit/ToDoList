using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.ToDos.Queries.Requests;
using ToDoList.Models.Dtos;

namespace ToDoList.ToDo.Queries.Handlers
{
    public class GetToDoItemsHandler : IRequestHandler<GetToDoItems, List<ToDoItemDto>>
    {
        private readonly IToDoDatabase _database;

        public GetToDoItemsHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<List<ToDoItemDto>> Handle(GetToDoItems request, CancellationToken cancellationToken)
        {
            var items = await _database.GetAllToDoItemsAsync(cancellationToken);
            return items;
        }
    }
}
