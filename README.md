# ToDoList

Repository for learning C# by working through a ToDoList application in C#. First, write a simple todo-application as console application. Next do a rest API. Then build it up step by step according to the assignments.

## History of assignments and related notes

1.  Replace all foreach loops with LINQ:

    Current implementation uses `FirstOrDefault` as follows:
    ```csharp
        public bool Update(int id, UpdateItemDto item)
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
            return false;
        }
    ```
    To use `First` instead of `FirstOrDefault`: `First` throws an exception if no item is found that matches the condition. If you expect that an item with the given id may not exist, it's generally safer to use `FirstOrDefault`. However, if you are sure that the item will exist and you want to use `First`, you should wrap it in a try-catch block to handle the potential `InvalidOperationException`. Using `FirstOrDefault` is the correct approach when the presence of the item isn't guaranteed. It avoids the unnecessary cost of exception handling which should not be used for normal control flow in your programs.

    Here's how you might use  `First`:
    ```csharp
    public bool Update(int id, UpdateItemDto item)
    {
        try
        {
            // Find the first item that matches the ID or throw an exception if none found.
            var itemToUpdate = _items.First(i => i.Id == id);
            
            if (item.Task != null)
            {
                itemToUpdate.Task = item.Task;
            }
            if (item.IsCompleted.HasValue)
            {
                itemToUpdate.IsCompleted = item.IsCompleted.Value;
            }

            // If we've reached here, the item has been successfully updated.
            return true;
        }
        catch (InvalidOperationException)
        {
            // If an exception was caught, it means no item matched the ID.
            return false;
        }
    }
    ```
2. Replace the inmemory database with an interface;
3. Add a seperate implementation for the database that uses a dictionary instead of a list.
4. Split out classes and files in different folders.
5. Update the code to handle concurrent updates. Assume 2 people are creating a new todo item at the same time. What to do with the ids? Think of the solution as multiple backends running against a central database. The database will then handle the concurrency. How to solve it here? 

    I found several solutions: 
    - `int newId = Interlocked.Increment(ref _nextId);`
    - `Id = Guid.NewGuid(),`
    - `lock (_lock) {}`
    
    which are complimentary in the sense that you can choose either `Guid` or `Interlocked.Increment` because they are both guaranteeing a unique Id. But additionally to control access to the "database" itself you need some kind of `lock` pattern.
    Whereas my first implementation included the lock in the form of"
    ```csharp
    private readonly object _lock = new();
    lock (_lock)
    {
        code to be locked
    }
    ``` 
    You don't need a new object `_lock` for this as you can just use `lock(this)` instead.
6. Dependency inject which database to use.
 

[TODISCUSS:]

7. Stretch: Implement the Mediator Pattern. Look for inspiration in the existing code base.
Mediatr.
Request handler
Queries/Commands: read/update

8. Unit tests.
9. making sure that the interface down to the database is async.
--
Later:
look at async/await.