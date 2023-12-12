using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.People.Queries.Requests;

namespace ToDoList.People.Queries.Handlers
{
    public class GetPeopleByToDoItemHandler : IRequestHandler<GetPeopleByToDoItem, List<PersonDto>>
    {
        private readonly IToDoDatabase _database;

        public GetPeopleByToDoItemHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<List<PersonDto>> Handle(GetPeopleByToDoItem request, CancellationToken cancellationToken)
        {
            var people = await _database.GetPeopleByToDoItemAsync(request.ItemId, cancellationToken);
            return people;
        }
    }
}
