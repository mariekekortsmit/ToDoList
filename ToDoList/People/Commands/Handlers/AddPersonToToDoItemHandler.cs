using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;
using ToDoList.People.Commands.Requests;

namespace ToDoList.People.Commands.Handlers
{
    public class AddPersonToToDoItemHandler : IRequestHandler<AddPersonToToDoItem, bool>
    {
        private readonly IToDoDatabase _database;

        public AddPersonToToDoItemHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Handle(AddPersonToToDoItem request, CancellationToken cancellationToken)
        {
            bool added = await _database.AddPersonToToDoItemAsync(request.ItemId, request.PersonId, cancellationToken);
            return added;
        }

    }
}
