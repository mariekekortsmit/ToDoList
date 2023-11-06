using ToDoList.Models;
using ToDoList.DataAccess;

namespace ToDoList.Services
{
    public class InMemoryToDoDatabaseList : IToDoDatabase
    {
        private readonly List<ToDoItem> _items = new();
        private int _nextId = 1;

        // Retrieve all items.
        public List<ToDoItemDto> GetAll() => _items.Select(x => x.ToDto()).ToList();

        // Find an item by Id.
        public ToDoItemDto? Get(int id)
        {
            return _items.First(item => item.Id == id).ToDto();
        }

        // Add a new item.
        public ToDoItem Add(AddItemDto item)
        {
            _items.Add(new ToDoItem() { Id = _nextId++, Task = item.Task, IsCompleted = item.IsCompleted });
            return _items.Last();
        }

        // Update an existing item.
        public bool Update(int id, UpdateItemDto item)
        {
            var itemToUpdate = _items.First(item => item.Id == id);
            if (itemToUpdate != null)
            {
                if (item.Task != null)
                {
                    itemToUpdate.Task = item.Task;
                }
                if (item.IsCompleted.HasValue)
                {
                    itemToUpdate.IsCompleted = item.IsCompleted.Value;
                }
                return true;
            }
            return false;
        }

        // Delete an item by Id.
        public bool Delete(int id)
        {
            var itemToDelete = _items.First(item => item.Id == id);
            if (itemToDelete != null)
            {
                _items.Remove(itemToDelete);
                return true;
            }
            return false;
        }
    }
}
