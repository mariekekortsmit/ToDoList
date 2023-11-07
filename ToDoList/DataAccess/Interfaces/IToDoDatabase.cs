using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Interfaces
{
    public interface IToDoDatabase
    {
        List<ToDoItemDto> GetAll();
        ToDoItemDto? Get(Guid id);
        ToDoItem Add(AddItemDto item);
        bool Update(Guid id, UpdateItemDto item);
        bool Delete(Guid id);
    }
}
