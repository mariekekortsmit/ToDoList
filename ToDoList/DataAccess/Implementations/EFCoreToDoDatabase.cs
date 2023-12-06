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

        public async Task<List<ToDoItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ToDoItems
                .Select(item => item.ToDto())
                .ToListAsync(cancellationToken);
        }

        public async Task<ToDoItemDto?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { id }, cancellationToken);
            return item?.ToDto();
        }

        public async Task<ToDoItem> AddAsync(AddItemDto itemDto, CancellationToken cancellationToken)
        {
            var newItem = new ToDoItem { Task = itemDto.Task, IsCompleted = itemDto.IsCompleted };
            _context.ToDoItems.Add(newItem);
            await _context.SaveChangesAsync(cancellationToken);
            return newItem;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateItemDto itemDto, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { id }, cancellationToken);
            if (item == null) return false;

            item.Task = itemDto.Task ?? item.Task;
            item.IsCompleted = itemDto.IsCompleted ?? item.IsCompleted;

            _context.ToDoItems.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.ToDoItems.FindAsync(new object[] { id }, cancellationToken);
            if (item == null) return false;

            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
