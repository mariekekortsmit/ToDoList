using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class InMemoryToDoDatabaseDict : IToDoDatabase
    {
        private readonly Dictionary<Guid, ToDoItem> _items = new();
        private readonly object _lock = new();


        // Retrieve all items.
        public List<ToDoItemDto> GetAll()
        {
            lock (_lock)
            {
                return _items.Values.Select(x => x.ToDto()).ToList();
            }
        }

        // Find an item by Id.
        public ToDoItemDto? Get(Guid id)
        {
            lock (_lock)
            {
                var found = _items.TryGetValue(id, out ToDoItem? value);
                return found ? value?.ToDto() : null;
            }
        }

        // Add a new item.
        public ToDoItem Add(AddItemDto item)
        {
            var newItem = new ToDoItem() { Id = Guid.NewGuid(), Task = item.Task, IsCompleted = item.IsCompleted };
            lock (_lock)
            {
                _items.Add(newItem.Id, newItem);
            }
            return newItem;
        }

        // Update an existing item.
        public bool Update(Guid id, UpdateItemDto item)
        {
            lock (_lock)
            {
                // Attempt to retrieve the item with the specified id.
                if (_items.TryGetValue(id, out var itemToUpdate))
                {
                    // If the item was found, proceed with the update.
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
            // If the item was not found, return false.
            return false;
        }

        // Delete an item by Id.
        public bool Delete(Guid id)
        {
            lock (_lock)
            {
                // Check if the item exists in the dictionary before attempting to delete.
                if (_items.TryGetValue(id, out _))
                {
                    // The item exists, so remove it from the dictionary.
                    _items.Remove(id);
                    return true;
                }
            }
            // The item was not found, so there is nothing to delete.
            return false;
        }

    }
}
