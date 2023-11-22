using System.Threading;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class InMemoryToDoDatabaseList : IToDoDatabase
    {
        private readonly List<ToDoItem> _items = new();
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
                    return _items.Select(x => x.ToDto()).ToList();
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
                    if(cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }
                    return _items.FirstOrDefault(item => item.Id == id)?.ToDto();
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
                    _items.Add(newItem);
                    return newItem;
                });
            }
            finally { _semaphore.Release(); }
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
                    var itemToDelete = _items.FirstOrDefault(item => item.Id == id);
                    if (itemToDelete != null)
                    {
                        _items.Remove(itemToDelete);
                        return true;
                    }
                    return false;
                 });
            }

            finally { _semaphore.Release(); }
        }
    }
}
          