using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class InMemoryToDoDatabaseList : IToDoDatabase
    {
        private readonly List<ToDoItem> _items = new();
        private readonly object _lock = new();

        // Retrieve all items.
        public List<ToDoItemDto> GetAll()
        {
            lock (_lock)
            {
                return _items.Select(x => x.ToDto()).ToList();
            }
        }        

        // Find an item by Id.
        public ToDoItemDto? Get(Guid id)
        {
            lock (_lock)
            {
                return _items.FirstOrDefault(item => item.Id == id)?.ToDto();
            }
        }

        // Add a new item.
        public ToDoItem Add(AddItemDto item)
        {
            var newItem = new ToDoItem() { Id = Guid.NewGuid(), Task = item.Task, IsCompleted = item.IsCompleted };
            lock (_lock)
            {
                _items.Add(newItem);
            }
            return newItem;
        }

        // Update an existing item.
        public bool Update(Guid id, UpdateItemDto item)
        {
            lock (_lock)
            {
                var itemToUpdate = _items.FirstOrDefault(item => item.Id == id);
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
            }
            return false;
        }

        // Delete an item by Id.
        public bool Delete(Guid id)
        {
            lock (_lock)
            {
                var itemToDelete = _items.FirstOrDefault(item => item.Id == id);
                if (itemToDelete != null)
                {
                    _items.Remove(itemToDelete);
                    return true;
                }
            }
            return false;
        }
    }
}
