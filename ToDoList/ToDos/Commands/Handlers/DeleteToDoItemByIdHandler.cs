using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.ToDos.Queries.Requests;

namespace ToDoList.ToDos.Commands.Handlers
{
    public class DeleteToDoItemByIdHandler : IRequestHandler<DeleteToDoItemById, bool>
    {
        private readonly IToDoDatabase _database;

        public DeleteToDoItemByIdHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Handle(DeleteToDoItemById request, CancellationToken cancellationToken)
        {
            bool deleted = await _database.DeleteAsync(request.Id, cancellationToken);
            return deleted;
        }
    }
}
