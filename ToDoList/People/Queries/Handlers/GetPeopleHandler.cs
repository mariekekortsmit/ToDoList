using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.People.Queries.Requests;
using ToDoList.Models.Dtos;

namespace ToDoList.People.Queries.Handlers
{
    public class GetPeopleHandler : IRequestHandler<GetPeople, List<PersonDto>>
    {
        private readonly IToDoDatabase _database;

        public GetPeopleHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<List<PersonDto>> Handle(GetPeople request, CancellationToken cancellationToken)
        {
            var people = await _database.GetAllPeopleAsync(cancellationToken);
            return people;
        }
    }
}
