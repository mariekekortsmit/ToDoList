using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.People.Commands.Requests;

namespace ToDoList.People.Commands.Handlers
{
    public class DeletePersonByIdHandler : IRequestHandler<DeletePersonById, bool>
    {
        private readonly IToDoDatabase _database;

        public DeletePersonByIdHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Handle(DeletePersonById request, CancellationToken cancellationToken)
        {
            bool deleted = await _database.DeletePersonAsync(request.Id, cancellationToken);
            return deleted;
        }
    }
}
