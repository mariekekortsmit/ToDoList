using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Entities;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationship
        modelBuilder.Entity<ToDoItem>()
            .HasMany(t => t.People)
            .WithMany(p => p.ToDoItems);
    }
}
