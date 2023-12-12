using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.People.Commands.Requests;

namespace ToDoList.ToDos.Commands.Handlers
{
    public class PutPersonByIdHandler : IRequestHandler<PutPersonById, bool>
    {
        private readonly IToDoDatabase _database;

        public PutPersonByIdHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Handle(PutPersonById request, CancellationToken cancellationToken)
        {
            bool updated = await _database.UpdatePersonAsync(request.Id, request.Person, cancellationToken);
            return updated;
        }

    }
}
