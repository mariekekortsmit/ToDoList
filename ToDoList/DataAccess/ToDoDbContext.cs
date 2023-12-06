using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Entities;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }
}
