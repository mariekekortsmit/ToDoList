using Microsoft.EntityFrameworkCore;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Implementations
{
    public class EFCoreToDoDatabase : IToDoDatabase
    {
        private readonly ToDoDbContext _context;

        public EFCoreToDoDatabase(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDoItemDto>> GetAllToDoItemsAsync(CancellationToken cancellationToken)
        {
            return await _context.ToDoItems
                .Select(item => item.ToDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<ToDoItemDto?> GetToDoItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { id }, cancellationToken);
            return item?.ToDto();
        }

        public async Task<List<ToDoItemDto>> GetToDoItemsByPersonAsync(Guid personId, CancellationToken cancellationToken)
        {
            var person = await _context.People.FindAsync(new object[] { personId }, cancellationToken);
            if (person == null) return new List<ToDoItemDto>();
            return await _context.ToDoItems
                .Where(item => item.People.Contains(person))
                .Select(item => item.ToDto())
                .ToListAsync(cancellationToken);
        }   

        public async Task<ToDoItem> AddToDoItemAsync(AddItemDto itemDto, CancellationToken cancellationToken)
        {
            var newItem = new ToDoItem { Task = itemDto.Task, IsCompleted = itemDto.IsCompleted };
            _context.ToDoItems.Add(newItem);
            await _context.SaveChangesAsync(cancellationToken);
            return newItem;
        }

        public async Task<bool> UpdateToDoItemAsync(Guid id, UpdateItemDto itemDto, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { id }, cancellationToken);
            if (item == null) return false;

            item.Task = itemDto.Task ?? item.Task;
            item.IsCompleted = itemDto.IsCompleted ?? item.IsCompleted;

            _context.ToDoItems.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteToDoItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { id }, cancellationToken);
            if (item == null) return false;

            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<PersonDto>> GetAllPeopleAsync(CancellationToken cancellationToken)
        {
            return await _context.People
                .Select(person => person.ToDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<PersonDto?> GetPersonAsync(Guid id, CancellationToken cancellationToken)
        {
            var person = await _context.People.FindAsync(new object[] { id }, cancellationToken);
            return person?.ToDto();
        }  

        public async Task<List<PersonDto>> GetPeopleByToDoItemAsync(Guid itemId, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { itemId }, cancellationToken);
            if (item == null) return new List<PersonDto>();
            return await _context.People
                .Where(person => person.ToDoItems.Contains(item))
                .Select(person => person.ToDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<Person> AddPersonAsync(AddPersonDto personDto, CancellationToken cancellationToken)
        {
            var newPerson = new Person { FirstName = personDto.FirstName, LastName = personDto.LastName };
            _context.People.Add(newPerson);
            await _context.SaveChangesAsync(cancellationToken);
            return newPerson;
        }

        public async Task<bool> UpdatePersonAsync(Guid id, UpdatePersonDto personDto, CancellationToken cancellationToken)
        {
            var person = await _context.People.FindAsync(new object[] { id }, cancellationToken);
            if (person == null) return false;

            person.FirstName = personDto.FirstName ?? person.FirstName;
            person.LastName = personDto.LastName ?? person.LastName;

            _context.People.Update(person);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeletePersonAsync(Guid id, CancellationToken cancellationToken)
        {
            var person = await _context.People.FindAsync(new object[] { id }, cancellationToken);
            if (person == null) return false;

            _context.People.Remove(person);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AddPersonToToDoItemAsync(Guid itemId, Guid personId, CancellationToken cancellationToken)
        {
            var personToAddToTask = await _context.People.FindAsync(new object[] { personId }, cancellationToken);
            if (personToAddToTask == null) return false;
            var itemToUpdate = await _context.ToDoItems.FindAsync(new object[] { itemId }, cancellationToken);
            if(itemToUpdate == null) return false;
            itemToUpdate.People.Add(personToAddToTask);
            personToAddToTask.ToDoItems.Add(itemToUpdate);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeletePersonFromToDoItemAsync(Guid itemId, Guid personId, CancellationToken cancellationToken)
        {
            var personToRemoveFromTask = await _context.People.FindAsync(new object[] { personId }, cancellationToken);
            if (personToRemoveFromTask == null) return false;
            var itemToUpdate = await _context.ToDoItems.FindAsync(new object[] { itemId }, cancellationToken);
            if (itemToUpdate == null) return false;
            itemToUpdate.People.Remove(personToRemoveFromTask);
            personToRemoveFromTask.ToDoItems.Remove(itemToUpdate);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
