using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.ToDos.Commands.Handlers
{
    public class PutToDoItemByIdHandler : IRequestHandler<PutToDoItemById, bool>
    {
        private readonly IToDoDatabase _database;

        public PutToDoItemByIdHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public Task<bool> Handle(PutToDoItemById request, CancellationToken cancellationToken)
        {
            bool updated = _database.Update(request.Id, request.Item);
            return Task.FromResult(updated);
        }

    }
}
