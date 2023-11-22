using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Interfaces
{
    public interface IToDoDatabase
    {
        Task<List<ToDoItemDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ToDoItemDto?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<ToDoItem> AddAsync(AddItemDto item, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Guid id, UpdateItemDto item, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
