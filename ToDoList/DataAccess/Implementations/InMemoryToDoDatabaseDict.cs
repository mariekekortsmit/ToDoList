using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class InMemoryToDoDatabaseDict : IToDoDatabase
    {
        private readonly Dictionary<Guid, ToDoItem> _items = new();
        private readonly Dictionary<Guid, Person> _people = new();
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
                    return _items.Values.Select(x => x.ToDto()).ToList();
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
                    var found = _people.TryGetValue(personId, out Person? value);
                    if (found && value != null && value.ToDoItems != null)
                    {
                        return value.ToDoItems.Select(todoItem => todoItem.ToDto()).ToList();
                    }
                    else
                    {
                        return new List<ToDoItemDto>(); // Return an empty list
                    }

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
                    _items.Add(newItem.Id, newItem);
                    return newItem;
                }); 
            }
            finally { _semaphore.Release();  }
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

        // Retrieve all items.
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
                    return _people.Values.Select(x => x.ToDto()).ToList();
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
                    var found = _people.TryGetValue(id, out Person? value);
                    return found ? value?.ToDto() : null;
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
                    var found = _items.TryGetValue(itemId, out ToDoItem? value);
                    if (found && value != null && value.People != null)
                    {
                        return value.People.Select(person => person.ToDto()).ToList();
                    }
                    else
                    {
                        return new List<PersonDto>(); // Return an empty list
                    }

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
                    _people.Add(newPerson.Id, newPerson);
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
                    // Attempt to retrieve the item with the specified id.
                    if (_people.TryGetValue(id, out var personToUpdate))
                    {
                        // If the item was found, proceed with the update.
                        if (person.FirstName != null)
                        {
                            personToUpdate.FirstName = person.FirstName;
                        }
                        if (person.LastName != null)
                        {
                            personToUpdate.LastName = person.LastName;
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
                    // Check if the item exists in the dictionary before attempting to delete.
                    if (_people.TryGetValue(id, out _))
                    {
                        // The item exists, so remove it from the dictionary.
                        _people.Remove(id);
                        return true;
                    }
                    // The item was not found, so there is nothing to delete.
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
                    // Attempt to retrieve the item with the specified id.
                    if (_items.TryGetValue(itemId, out var itemToUpdate))
                    {
                        // If the item was found, proceed with the update.
                        if (_people.TryGetValue(personId, out var person))
                        {
                            itemToUpdate.People.Add(person);
                            person.ToDoItems.Add(itemToUpdate);
                            return true;
                        }
                    }
                    // If the item was not found, return false.
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
                    // Attempt to retrieve the item with the specified id.
                    if (_items.TryGetValue(itemId, out var itemToUpdate))
                    {
                        // If the item was found, proceed with the update.
                        if (_people.TryGetValue(personId, out var person))
                        {
                            itemToUpdate.People.Remove(person);
                            person.ToDoItems.Remove(itemToUpdate);
                            return true;
                        }
                    }
                    // If the item was not found, return false.
                    return false;
                }); 
            }
            finally { _semaphore.Release();}
        }
    }
}
