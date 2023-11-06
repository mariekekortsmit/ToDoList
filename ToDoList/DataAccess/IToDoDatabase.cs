using ToDoList.Models;

namespace ToDoList.DataAccess
{
    public interface IToDoDatabase
    {
        List<ToDoItemDto> GetAll();
        ToDoItemDto? Get(int id);
        ToDoItem Add(AddItemDto item);
        bool Update(int id, UpdateItemDto item);
        bool Delete(int id);
    }
}
