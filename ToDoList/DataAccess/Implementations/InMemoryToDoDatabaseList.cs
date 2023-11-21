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
                return _items.Select(x => x.ToDto()).ToList();
            }
            finally { _semaphore.Release(); }
        }

        // Find an item by Id.
        public async Task<ToDoItemDto?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return _items.FirstOrDefault(item => item.Id == id)?.ToDto();
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
                _items.Add(newItem);
            }
            finally { _semaphore.Release(); }

            return newItem;
        }

        // Update an existing item.
        public async Task<bool> UpdateAsync(Guid id, UpdateItemDto item, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
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
            finally { _semaphore.Release(); }

            return false;
        }

        // Delete an item by Id.
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var itemToDelete = _items.FirstOrDefault(item => item.Id == id);
                if (itemToDelete != null)
                {
                    _items.Remove(itemToDelete);
                    return true;
                }
            }

            finally { _semaphore.Release(); }

            return false;
        }
    }
}
