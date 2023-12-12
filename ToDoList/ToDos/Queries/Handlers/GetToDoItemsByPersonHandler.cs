using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.ToDos.Queries.Requests;

namespace ToDoList.ToDos.Queries.Handlers
{
    public class GetToDoItemsByPersonHandler : IRequestHandler<GetToDoItemsByPerson, List<ToDoItemDto>>
    {
        private readonly IToDoDatabase _database;

        public GetToDoItemsByPersonHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<List<ToDoItemDto>> Handle(GetToDoItemsByPerson request, CancellationToken cancellationToken)
        {
            var items = await _database.GetToDoItemsByPersonAsync(request.PersonId, cancellationToken);
            return items;
        }
    }
}
