using System.Threading;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class InMemoryToDoDatabaseList : IToDoDatabase
    {
        private readonly List<ToDoItem> _items = new();
        private readonly List<Person> _people = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        // Retrieve all items.
        public async Task<List<ToDoItemDto>> GetAllToDoItemsAsync(CancellationToken cancellationToken)
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
        public async Task<ToDoItemDto?> GetToDoItemAsync(Guid id, CancellationToken cancellationToken)
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

        // Retrieve all items by person id.
        public async Task<List<ToDoItemDto>> GetToDoItemsByPersonAsync(Guid personId, CancellationToken cancellationToken)
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
                    var person = _people.FirstOrDefault(i => i.Id == personId);
                    if (person == null)
                    {
                        return new List<ToDoItemDto>(); // Return an empty list if person is not found
                    }

                    return person.ToDoItems.Select(item => item.ToDto()).ToList();
                });
            }
            finally { _semaphore.Release(); }
        }

        // Add a new item.
        public async Task<ToDoItem> AddToDoItemAsync(AddItemDto item, CancellationToken cancellationToken)
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
        public async Task<bool> UpdateToDoItemAsync(Guid id, UpdateItemDto item, CancellationToken cancellationToken)
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
        public async Task<bool> DeleteToDoItemAsync(Guid id, CancellationToken cancellationToken)
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

        public async Task<List<PersonDto>> GetAllPeopleAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<List<PersonDto>>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new List<PersonDto>(); // Return an empty list
                    }
                    return _people.Select(x => x.ToDto()).ToList();
                });
            }
            finally { _semaphore.Release(); }
        }

        // Find an item by Id.
        public async Task<PersonDto?> GetPersonAsync(Guid id, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<PersonDto?>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }
                    return _people.FirstOrDefault(item => item.Id == id)?.ToDto();
                });
            }
            finally { _semaphore.Release(); }
        }


        // Retrieve all items by person id.
        public async Task<List<PersonDto>> GetPeopleByToDoItemAsync(Guid itemId, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<List<PersonDto>>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new List<PersonDto>(); // Return an empty list
                    }
                    var item = _items.FirstOrDefault(i => i.Id == itemId);
                    if (item == null)
                    {
                        return new List<PersonDto>(); // Return an empty list if item is not found
                    }

                    return item.People.Select(person => person.ToDto()).ToList();
                });
            }
            finally { _semaphore.Release(); }
        }

        // Add a new item.
        public async Task<Person> AddPersonAsync(AddPersonDto person, CancellationToken cancellationToken)
        {
            var newPerson = new Person() { Id = Guid.NewGuid(), FirstName = person.FirstName, LastName = person.LastName };
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return await Task.Run<Person>(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new Person(); // Return an empty item
                    }
                    _people.Add(newPerson);
                    return newPerson;
                });
            }
            finally { _semaphore.Release(); }
        }

        // Update an existing item.
        public async Task<bool> UpdatePersonAsync(Guid id, UpdatePersonDto person, CancellationToken cancellationToken)
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
                    var personToUpdate = _people.FirstOrDefault(person => person.Id == id);
                    if (personToUpdate != null)
                    {
                        if(person.FirstName != null)
                        {
                            personToUpdate.FirstName = person.FirstName;
                        }
                        if(person.LastName != null)
                        {
                            personToUpdate.LastName = person.LastName;
                        }
                        return true;
                    }
                    return false;
                });
            }
            finally { _semaphore.Release(); }
        }

        // Delete an item by Id.
        public async Task<bool> DeletePersonAsync(Guid id, CancellationToken cancellationToken)
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
                    var personToDelete = _people.FirstOrDefault(person => person.Id == id);
                    if (personToDelete != null)
                    {
                        _people.Remove(personToDelete);
                        return true;
                    }
                    return false;
                });
            }

            finally { _semaphore.Release(); }
        }

        public async Task<bool> AddPersonToToDoItemAsync(Guid itemId, Guid personId, CancellationToken cancellationToken)
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
                    var personToAddToTask = _people.FirstOrDefault(person => person.Id == personId);
                    if (personToAddToTask != null)
                    {
                        var itemToUpdate = _items.FirstOrDefault(item => item.Id == itemId);
                        if(itemToUpdate != null)
                        {
                            itemToUpdate.People.Add(personToAddToTask);
                            personToAddToTask.ToDoItems.Add(itemToUpdate);
                            return true;
                        }
                    }
                    // If the person was not found, return false
                    return false;
                });
            }
            finally { _semaphore.Release(); }
        }

        public async Task<bool> DeletePersonFromToDoItemAsync(Guid itemId, Guid personId, CancellationToken cancellationToken)
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
                    var personToRemoveFromTask = _people.FirstOrDefault(person => person.Id == personId);
                    if (personToRemoveFromTask != null)
                    {
                        var itemToUpdate = _items.FirstOrDefault(item => item.Id == itemId);
                        if (itemToUpdate != null)
                        {
                            itemToUpdate.People.Remove(personToRemoveFromTask);
                            personToRemoveFromTask.ToDoItems.Remove(itemToUpdate);
                            return true;
                        }
                    }
                    // If the person was not found, return false
                    return false;
                });
            }
            finally { _semaphore.Release(); }
        }
    }
}

          