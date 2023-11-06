using ToDoList.DataAccess;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class InMemoryToDoDatabaseDict : IToDoDatabase
    { 
        private readonly Dictionary<int, ToDoItem> _items = new();
        private int _nextId = 1;

        // Retrieve all items.
        public List<ToDoItemDto> GetAll() => _items.Values.Select(x => x.ToDto()).ToList();

        // Find an item by Id.
        public ToDoItemDto? Get(int id)
        {
            return _items[id].ToDto();
        }

        // Add a new item.
        public ToDoItem Add(AddItemDto item)
        {
            var id = _nextId++;
            _items.Add(
                id,
                new ToDoItem() { 
                    Id = id, 
                    Task = item.Task, 
                    IsCompleted = item.IsCompleted 
                }
            );
            return _items[id];
        }

        // Update an existing item.
        public bool Update(int id, UpdateItemDto item)
        {
            var itemToUpdate = _items[id];
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
            var itemToDelete = _items[id];
            if (itemToDelete != null)
            {
                _items.Remove(id);
                return true;
            }
            return false;
        }
    }
}
