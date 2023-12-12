# 🌟 Welcome to my Journey of Learning C# with ToDoList! 🌟

🎉 Hey there! Welcome to my adventure in the world of C# programming! This repository isn't just about coding; it's about me (and potentially you) enjoying the journey of building a ToDoList application from the ground up. 🚀

Here's how you can join in the fun:

1. Start Simple: Kick off with a console-based ToDoList app. It's like the "Hello, World!" of the project.
1. Level Up: Transform your simple app into a feature-rich masterpiece, step by step.
1. Explore and Discover: Each assignment is a new quest in the land of C#, complete with my own treasure maps (notes) and secrets I've unearthed along the way. 🗺️

Expect to find:

- 🤔 The puzzles I solved.
- 💡 My 'Aha!' moments.
- 📚 Personal learnings, packaged in a mix of challenges and triumphs.

## Let the adventure begin

ToDoList: Your Ticket to Coding Adventures! Embark on this quest to conquer the world of C# with our ToDoList app. Each challenge you complete is a badge of honor, a story to tell. 

Ready? Set? Code! 🌈👨‍💻👩‍💻🌈

1.  **Write a simple ToDoLits application as console application.**
1.  **Convert the solution to use REST API.**
1.  **Replace all foreach loops with LINQ:**

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
1.  **Replace the inmemory database with an interface;**
1.  **Add a seperate implementation for the database that uses a dictionary instead of a list.**
1.  **Split out classes and files in different folders.**
1.  **Update the code to handle concurrent updates.** Assume 2 people are creating a new todo item at the same time. What to do with the ids? Think of the solution as 
    multiple backends running against a central database. The database will then handle the concurrency. How to solve it here? 

    I found several solutions: 
    - `int newId = Interlocked.Increment(ref _nextId);`
    - `Id = Guid.NewGuid(),`s
    - `lock (_lock) {}`
    
    which are complimentary in the sense that you can choose either `Guid` or `Interlocked.Increment` because they are both guaranteeing a unique Id. But additionally to control access to the "database" itself you need some kind of `lock` pattern.
    Whereas my first implementation included the lock in the form of"
    ```csharp
    private readonly object _lock = new();
    lock (_lock)
    {
        // code to be locked
    }
    ``` 
    You don't need a new object `_lock` for this as you can just use `lock(this)` instead.
1.  **Dependency inject which database to use.**
    Dependency injecion in ASP.NET Core has 3 service lifetimes: Singleton, Scoped and Trasient. Here is an overview of their respective lifetimes and common use cases:
    |   | Singleton | Scoped    | Transient |
    |---|-----------|-----------|-----------|
    | Lifetime Description  | Created once and shared throughout the application's life | Created anew for each client request | Created each time they are requested |
    | Common Use Cases | - Stateless services<br>- Config and logging services<br>- Maintaining global shared state | - Database contexts (e.g., Entity Framework)<br>- User-specific information processing<br>- Operations requiring separate instances per request | - Lightweight, stateless services<br>- Services where each operation is distinct and does not maintain state |

    Therefore the current List and Dict databases are aded with Singleton.
1.  **Implement the Mediater Pattern.** Use the `Mediatr` package.
1.  **Unit tests.**
    
    - *Using the interface to test both db implementations in the same way.*

        At first, I just unit tested one implementation of my database, the dictionary version, with something that looked like this:
        ```csharp
        public class InMemoryToDoDatabaseDictTests
        {
            [Fact]
            public void GetAll_NonEmptyDatabase_ShouldReturnAllItems()
            {
                // Arrange
                var database = new InMemoryToDoDatabaseDict();
                var item1 = database.Add(new AddItemDto { Task = "Task 1", IsCompleted = false });
                var item2 = database.Add(new AddItemDto { Task = "Task 2", IsCompleted = true });

                // Act
                var result = database.GetAll();

                // Assert
                result.Should().HaveCount(2);
                result.Should().Contain(x => x.Id == item1.Id && x.Task == "Task 1" && x.IsCompleted == false);
                result.Should().Contain(x => x.Id == item2.Id && x.Task == "Task 2" && x.IsCompleted == true);
            }
        ```
        However, changing to this implementation of unit tests makes that you can use the interface to test both implementations at once:
        ```csharp
        public class InMemoryToDoDatabaseDictTests : InMemoryToDoDatabaseTests<InMemoryToDoDatabaseDict> { }
        public class InMemoryToDoDatabaseListTests : InMemoryToDoDatabaseTests<InMemoryToDoDatabaseList> { }

        public abstract class InMemoryToDoDatabaseTests<T> where T: IToDoDatabase, new()
        {
            [Fact]
            public void GetAll_NonEmptyDatabase_ShouldReturnAllItems()
            {
                // Arrange
                var database = new T();
                // ..
            }
        }
        ```

    - *Mocks in unit testing: *
        
        The first implementation of unit tests included a mocked database based on the interface like this:
        ```csharp
        public async Task Handle_ValidRequest_AddsItem()
            {
                // Arrange
                var mockDatabase = new Mock<IToDoDatabase>();
                var addItemDto = new AddItemDto { Task = "Test Task", IsCompleted = false };
                var expectedToDoItem = new ToDoItem { Id = Guid.NewGuid(), Task = "Test Task", IsCompleted = false };
                mockDatabase.Setup(db => db.Add(addItemDto)).Returns(expectedToDoItem);
                var handler = new AddToDoItemHandler(mockDatabase.Object);

                var request = new AddToDoItem(addItemDto);

                // Act
                var result = await handler.Handle(request, CancellationToken.None);

                // Assert
                result.Should().BeEquivalentTo(expectedToDoItem);
                mockDatabase.Verify(db => db.Add(addItemDto), Times.Once);
            }
        ```
        However, as the database implementations should be fully tested, you don't need to mock the actual database in your other tests as you can then trust this is doing the correct thing (you've covered this in your other unit tests).
1.  **Learn and implement Asynchronous Programming.** 
    - **Learn**: to understand the basics of async programming, I've created a Console Application in the LearningAsync folder, together with a README on all the learnings. 
    - **Implement**: make the database interface and implementations asynchronous, including taking in and checking a CancellationToken. Also, write the tests for that interface asynchronous.
    
    My first attempt at implementing the async setup looked like this for one of the database implementation functions:
    ```csharp
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
    ```
    where it acquires an async lock before executing the task. After acquiring that lock, it directly returns the transformed list. The transformation of the list items into `ToDoItemDto` objects and the conversion to a list is done within the same thread that called the method. However, if you implement it like this:
    ```csharp
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
                    return null;
                }
                return _items.Select(x => x.ToDto()).ToList();
            });
        }
        finally { _semaphore.Release(); }
    }
    ```
    it wraps the list transformation inside a `Task.Run`, meaning it is executed on a seperate thread from the thread pool. By using this approach it offloads the processing to a background thread which can be beneficial if the transformation itself takes a considerable amount of time. By offloading to a background thread, it keeps the calling thread (potentially the main UI thread) responsive.  
1.  **Learn and implement Entity Framework Core.** 
    - **Learn**: to understand the basics of EF core, I've created a very simple webapp in the LearningEF folder, together with a README on all the learnings.  
    - **Implement**: rewrite the application to use Entity Framework Core. To make this implementation a little bit more interesting I've added a Person entity with a many-to-many relationship between ToDos and People.  

        Notes:
        - I've used the code from my basic webapp in the LearningEF folder to start with. A few notes:
            - Adding the ConnectionStrings section to appsettings.Development.json and commit to git is safe as passwordless connection strings are safe to commit to source control, since they do not contain any secrets such as usernames, passwords, or access keys. Note that you'll have to change the connection string to refer to your own SQL server and database.
        - Since I don't want to delete my previous implementation of inmemory databases List and Dict, I moved those to a testing environment.
        - The SQL database is injected via dependency injection with `AddScoped`. In general, use `AddScoped` for Entity Framework database contexts in ASP.NET Core to ensure each HTTP request gets a fresh, isolated context. This approach efficiently manages resources, maintains data consistency across requests, and aligns with web application best practices.
        - Entity Framework not only creates tables for `People` and `ToDoItems`, but also automatically creates a join table in my scenario because:
            - Many-to-Many Relationship: the configuration in `OnModelCreating` specifies a many-to-many relationship between `ToDoItem` and `Person`.
            - No Join Entity Defined: since there's no explicit join entity (intermediate class) defined, Entity Framework generates a join table to handle the relationship. You can also explicitly define the join table, as described in this [doc](https://entityframework.net/many-to-many-relationship).
            - Database Normalization: This approach maintains database normalization by separating the two entities and handling their associations through a separate table.
