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

        public Task<ToDoItemDto?> Handle(GetToDoItemById request, CancellationToken cancellationToken)
        {
            var items = _database.Get(request.Id);
            return Task.FromResult(items);
        }
    }
}
