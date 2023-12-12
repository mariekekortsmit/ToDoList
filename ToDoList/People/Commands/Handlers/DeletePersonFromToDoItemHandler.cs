using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;
using ToDoList.People.Commands.Requests;

namespace ToDoList.People.Commands.Handlers
{
    public class DeletePersonFromToDoItemHandler : IRequestHandler<DeletePersonFromToDoItem, bool>
    {
        private readonly IToDoDatabase _database;

        public DeletePersonFromToDoItemHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Handle(DeletePersonFromToDoItem request, CancellationToken cancellationToken)
        {
            bool removed = await _database.DeletePersonFromToDoItemAsync(request.ItemId, request.PersonId, cancellationToken);
            return removed;
        }

    }
}
