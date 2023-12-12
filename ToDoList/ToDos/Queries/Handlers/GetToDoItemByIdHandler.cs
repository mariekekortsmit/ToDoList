using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.ToDos.Queries.Requests;

namespace ToDoList.ToDos.Queries.Handlers
{
    public class GetToDoItemByIdHandler : IRequestHandler<GetToDoItemById, ToDoItemDto?>
    {
        private readonly IToDoDatabase _database;

        public GetToDoItemByIdHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<ToDoItemDto?> Handle(GetToDoItemById request, CancellationToken cancellationToken)
        {
            var items = await _database.GetToDoItemAsync(request.Id, cancellationToken);
            return items;
        }
    }
}
