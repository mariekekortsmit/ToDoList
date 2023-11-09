using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.ToDos.Commands.Handlers
{
    // The command to add a new todo item
    /*public class AddToDoItemCommand : IRequest<ToDoItem>
    {
        public AddItemDto Item { get; }

        public AddToDoItemCommand(AddItemDto item)
        {
            Item = item;
        }
    }*/

    public class AddToDoHandler : IRequestHandler<AddToDo, ToDoItem>
    {
        private readonly IToDoDatabase _database;

        public AddToDoHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public Task<ToDoItem> Handle(AddToDo request, CancellationToken cancellationToken)
        {
            var newItem = _database.Add(request.Item);
            return Task.FromResult(newItem);
        }

    }
}
