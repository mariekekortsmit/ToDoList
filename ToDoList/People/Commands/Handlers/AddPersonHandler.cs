using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.People.Commands.Requests;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.People.Commands.Handlers
{
    public class AddPersonHandler : IRequestHandler<AddPerson, Person>
    {
        private readonly IToDoDatabase _database;

        public AddPersonHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<Person> Handle(AddPerson request, CancellationToken cancellationToken)
        {
            var newPerson = await _database.AddPersonAsync(request.Person, cancellationToken);
            return newPerson;
        }

    }
}
