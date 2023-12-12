using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.People.Queries.Requests;

namespace ToDoList.People.Queries.Handlers
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonById, PersonDto?>
    {
        private readonly IToDoDatabase _database;

        public GetPersonByIdHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<PersonDto?> Handle(GetPersonById request, CancellationToken cancellationToken)
        {
            var person = await _database.GetPersonAsync(request.Id, cancellationToken);
            return person;
        }
    }
}
