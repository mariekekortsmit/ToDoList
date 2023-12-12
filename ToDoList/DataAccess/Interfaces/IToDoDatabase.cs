using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Interfaces
{
    public interface IToDoDatabase
    {
        Task<List<ToDoItemDto>> GetAllToDoItemsAsync(CancellationToken cancellationToken);
        Task<ToDoItemDto?> GetToDoItemAsync(Guid itemId, CancellationToken cancellationToken);
        Task<List<ToDoItemDto>> GetToDoItemsByPersonAsync(Guid personId, CancellationToken cancellationToken);
        Task<ToDoItem> AddToDoItemAsync(AddItemDto item, CancellationToken cancellationToken);
        Task<bool> UpdateToDoItemAsync(Guid itemId, UpdateItemDto item, CancellationToken cancellationToken);
        Task<bool> DeleteToDoItemAsync(Guid itemId, CancellationToken cancellationToken);

        Task<List<PersonDto>> GetAllPeopleAsync(CancellationToken cancellationToken);
        Task<PersonDto?> GetPersonAsync(Guid personId, CancellationToken cancellationToken);
        Task<List<PersonDto>> GetPeopleByToDoItemAsync(Guid itemId, CancellationToken cancellationToken);
        Task<Person> AddPersonAsync(AddPersonDto person, CancellationToken cancellationToken);
        Task<bool> UpdatePersonAsync(Guid personId, UpdatePersonDto person, CancellationToken cancellationToken);
        Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken);
        Task<bool> AddPersonToToDoItemAsync(Guid itemId, Guid personId, CancellationToken cancellationToken);
        Task<bool> DeletePersonFromToDoItemAsync(Guid itemId, Guid personId, CancellationToken cancellationToken);

    }
}
