using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class InMemoryToDoDatabaseDict : IToDoDatabase
    {
        private readonly Dictionary<Guid, ToDoItem> _items = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);


        // Retrieve all items.
        public async Task<List<ToDoItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<List<ToDoItemDto>>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new List<ToDoItemDto>(); // Return an empty list
                    }
                    return _items.Values.Select(x => x.ToDto()).ToList();
                });
            }
            finally { _semaphore.Release(); }
        }

        // Find an item by Id.
        public async Task<ToDoItemDto?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<ToDoItemDto?>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }
                    var found = _items.TryGetValue(id, out ToDoItem? value);
                    return found ? value?.ToDto() : null;
                });
               
            }
            finally { _semaphore.Release(); }
        }

        // Add a new item.
        public async Task<ToDoItem> AddAsync(AddItemDto item, CancellationToken cancellationToken)
        {
            var newItem = new ToDoItem() { Id = Guid.NewGuid(), Task = item.Task, IsCompleted = item.IsCompleted };
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<ToDoItem>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new ToDoItem(); // Return an empty item
                    }
                    _items.Add(newItem.Id, newItem);
                    return newItem;
                }); 
            }
            finally { _semaphore.Release();  }
        }

        // Update an existing item.
        public async Task<bool> UpdateAsync(Guid id, UpdateItemDto item, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<bool>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return false;
                    }
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
                    // If the item was not found, return false.
                    return false;
                });
            }
            finally { _semaphore.Release(); }

        }

        // Delete an item by Id.
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<bool>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return false;
                    }
                    // Check if the item exists in the dictionary before attempting to delete.
                    if (_items.TryGetValue(id, out _))
                    {
                        // The item exists, so remove it from the dictionary.
                        _items.Remove(id);
                        return true;
                    }
                    // The item was not found, so there is nothing to delete.
                    return false;
                });

            }
            finally { _semaphore.Release(); }
        }

    }
}
